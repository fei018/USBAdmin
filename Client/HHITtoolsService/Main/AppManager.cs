using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgentLib;
using AgentLib.Win32API;

namespace HHITtoolsService
{
    public class AppManager
    {
        private static List<AppProcessInfo> _AppProcessList_Multiple => new List<AppProcessInfo>();

        private static Dictionary<string, AppProcessInfo> _AppProcessList_Singleton => new Dictionary<string, AppProcessInfo>();

        private const int _timeoutMillisecond = 2000;

        //

        #region Start()
        public void Start()
        {
            AppManager_Entity.Initial();

            //
            new AgentHttpHelp().PostPerComputer_Http();

            // PipeServer_Service
            AppManager_Entity.PipeServer_Service?.Start();

            // HHITtoolsUSB
            try
            {
                if (AgentRegistry.UsbFilterEnabled)
                {
                    Startup_HHITtoolsUSB();
                }
            }
            catch (Exception) { }

            // PrintJobNotify
            try
            {
                if (AgentRegistry.PrintJobHistoryEnabled)
                {
                    //Startup_PrintJobNotify();
                }
            }
            catch (Exception) { }

            // HHITtoolsTray
            Startup_HHITtoolsTray();

            // AppTimer
            //AppManager_Entity.AppTimer.ElapsedAction += AppTimer_ElapsedAction;
            //AppManager_Entity.AppTimer.Start();
        }
        #endregion

        #region Stop()
        public void Stop()
        {
            Close_HHITtoolsUSB();

            Close_HHITtoolsTray();

            //Close_PrintJobNotify();

            //AppManager_Entity.AppTimer.ElapsedAction -= AppTimer_ElapsedAction;
            //AppManager_Entity.AppTimer.Stop();

            AppManager_Entity.PipeServer_Service?.Stop();
        }
        #endregion

        #region + private void AppTimer_ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        private void AppTimer_ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    new AgentHttpHelp().PostPerComputer_Http();
                }
                catch (Exception ex)
                {
                    AgentLogger.Error("HHITtoolsService.AppManager.AppTimer_ElapsedAction(): " + ex.Message);
                }

                try
                {
                    new AgentHttpHelp().GetAgentSetting_Http();

                    if (AgentRegistry.UsbFilterEnabled)
                    {
                        if (!AppExist_Singleton(AgentRegistry.HHITtoolsUSBApp))
                        {
                            Startup_HHITtoolsUSB();
                        }
                    }

                    if (AgentRegistry.PrintJobHistoryEnabled)
                    {

                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error("HHITtoolsService.AppManager.AppTimer_ElapsedAction(): " + ex.Message);
                }
            });
        }
        #endregion

        //



        #region + private void CloseApp(string appFullPath)
        /// <summary>
        /// 強制 close app
        /// </summary>
        /// <param name="appFullPath"></param>
        private void CloseApp(string appFullPath)
        {
            try
            {
                var appinfos = _AppProcessDictionary.Keys.Where(k => k.AppFullPath == appFullPath);

                if (appinfos == null || appinfos.Count() <= 0)
                {
                    return;
                }

                foreach (var app in appinfos)
                {
                    if (_AppProcessDictionary.TryGetValue(app, out Process process))
                    {
                        CloseOrKillProcess(process);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + private bool AppExist_Single(string appPath)
        private bool AppExist_Singleton(string appPath)
        {
            try
            { 
                if (_AppProcessList_Singleton.TryGetValue(appPath, out AppProcessInfo appInfo))
                {
                    if (appInfo.ProcessExsit())
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        // HHITtoolsUSB

        #region + public static void Startup_HHITtoolsUSB()
        public static void Startup_HHITtoolsUSB()
        {
            string appPath = null;
            try
            {
                appPath = AgentRegistry.HHITtoolsUSBApp;
            }
            catch (Exception ex)
            {
                AgentLogger.Error("AgentRegistry.HHITtoolsUSBApp: " + ex.Message);
                return;
            }

            try
            {
                var appinfo = _AppProcessDictionary.Keys.FirstOrDefault(k => k.AppFullPath == appPath);
                if (appinfo != null)
                {
                    if (_AppProcessDictionary.TryGetValue(appinfo, out Process process))
                    {
                        CloseOrKillProcess(process);
                    }
                }

                var newProc = StartupAppAsSystem(AgentRegistry.HHITtoolsUSBApp);
                var newAppInfo = new AppProcessInfo
                {
                    AppFullPath = appPath,
                    ProcessId = newProc.Id
                };

                _AppProcessDictionary.Add(newAppInfo, newProc);
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        #region + public static void Close_HHITtoolsUSB()
        public static void Close_HHITtoolsUSB()
        {
            try
            {
                AppManager_Entity.PipeServer_Service?.SendMsg_CloseHHITtoolsUSB();
                Thread.Sleep(TimeSpan.FromMilliseconds(_timeoutMillisecond));
                CloseApp(AgentRegistry.HHITtoolsUSBApp);
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        // HHITtoolsTray

        #region + public static void Startup_HHITtoolsTray()
        public static void Startup_HHITtoolsTray()
        {
            string appPath = null;
            try
            {
                appPath = AgentRegistry.HHITtoolsTrayApp;
            }
            catch (Exception ex)
            {
                AgentLogger.Error("AgentRegistry.HHITtoolsTrayApp: " + ex.Message);
                return;
            }

            try
            {
#if DEBUG
                var proc = Process.Start(appPath);
#else
                var proc = StartupAppAsLogonUser(appPath);
#endif

                AppProcessInfo appinfo = new AppProcessInfo
                {
                    AppFullPath = appPath,
                    ProcessId = proc.Id
                };
                _AppProcessDictionary.Add(appinfo, proc);
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        #region + public static void Close_HHITtoolsTray()
        public static void Close_HHITtoolsTray()
        {
            try
            {
                AppManager_Entity.PipeServer_Service?.SendMsg_CloseHHITtoolsTray();
                Thread.Sleep(TimeSpan.FromMilliseconds(_timeoutMillisecond));
                CloseApp(AgentRegistry.HHITtoolsTrayApp);
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        // PrintJobNotify

        #region + public static void Startup_PrintJobNotify()
        public static void Startup_PrintJobNotify()
        {
            AppManager_Entity.PrintJobNotify?.Start();
        }
        #endregion

        #region + public static void Close_PrintJobNotify()
        public static void Close_PrintJobNotify()
        {
            AppManager_Entity.PrintJobNotify?.Stop();
        }
        #endregion

        #region + public static void Restart_PrintJobNotify()
        public static void Restart_PrintJobNotify()
        {
            AppManager_Entity.PrintJobNotify?.Restart();
        }
        #endregion

    }
}
