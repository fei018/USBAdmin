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
            // post computer info to server
            new AgentHttpHelp().PostPerComputer_Http();

            // PipeServer_Service
            AppManager_Entity.PipeServer_Service = new PipeServer_Service();
            AppManager_Entity.PipeServer_Service?.Start();

            // HHITtoolsUSB
            try
            {
                if (AgentRegistry.UsbFilterEnabled)
                {
                    AppManager_Entity.HHITtoolsUSBApp = AppProcessInfo.StartupApp(AgentRegistry.HHITtoolsUSBApp);
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
            var appTray = AppProcessInfo.StartupProcessAsLogonUser(AgentRegistry.HHITtoolsTrayApp);
            if(appTray != null) AppManager_Entity.HHITtoolsTrayList.Add(appTray);


            // AppTimer
            //AppManager_Entity.AppTimer.ElapsedAction += AppTimer_ElapsedAction;
            //AppManager_Entity.AppTimer.Start();
        }
        #endregion

        #region Stop()
        public void Stop()
        {
            Close_HHITtoolsUSB();

            HHITtoolsTray_Close();

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
                            HHITtoolsUSB_Startup();
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


        // Apps

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

        #region + public static void HHITtoolsUSB_Startup()
        public static void HHITtoolsUSB_Startup()
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
                AppManager_Entity.HHITtoolsUSBApp = AppProcessInfo.StartupApp(AgentRegistry.HHITtoolsUSBApp);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("AppManager.Startup_HHITtoolsUSB(): " + ex.Message);
            }
        }
        #endregion

        #region + public static void HHITtoolsUSB_Close()
        public static void HHITtoolsUSB_Close()
        {
            try
            {
                AppManager_Entity.PipeServer_Service?.SendMsg_CloseHHITtoolsUSB();
                Thread.Sleep(TimeSpan.FromMilliseconds(_timeoutMillisecond));

                AppManager_Entity.HHITtoolsUSBApp.CloseOrKillProcess();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        // HHITtoolsTray

        #region + public static void HHITtoolsTray_Startup()
        public static void HHITtoolsTray_Startup()
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
                var appTray = AppProcessInfo.StartupApp(appPath);
#else
                var appTray = AppProcessInfo.StartupProcessAsLogonUser(appPath);
#endif

                if(appTray != null) AppManager_Entity.HHITtoolsTrayList.Add(appTray);

            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        #region + public static void HHITtoolsTray_Close(int sessionId)
        public static void HHITtoolsTray_Close(int sessionId)
        {
            try
            {
                AppManager_Entity.PipeServer_Service?.SendMsg_CloseHHITtoolsTray(sessionId);
                Thread.Sleep(TimeSpan.FromMilliseconds(_timeoutMillisecond));

                AppManager_Entity.HHITtoolsTrayList.FirstOrDefault(a => a.AppProcess.SessionId == sessionId)?.CloseOrKillProcess();
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
