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
        private static readonly Dictionary<AppProcessInfo, Process> _AppProcessDictionary = new Dictionary<AppProcessInfo, Process>();

        private const int _timeoutMillisecond = 1000;

        //

        #region Startup()
        public static void Startup()
        {
            try
            {
                if (AgentRegistry.UsbFilterEnabled)
                {
                    Startup_HHITtoolsUSB();
                }
            }
            catch (Exception) { }

            try
            {
                if (AgentRegistry.PrintJobHistoryEnabled)
                {
                    Startup_PrintJobNotify();
                }
            }
            catch (Exception) { }     

            Startup_HHITtoolsTray();
        }
        #endregion

        #region Stop()
        public static void Stop()
        {
            Close_HHITtoolsUSB();

            Close_HHITtoolsTray();

            Close_PrintJobNotify();
        }
        #endregion

        //

        #region + private static Process StartupAppAsSystem(string appFullPath)
        private static Process StartupAppAsSystem(string appFullPath)
        {
            try
            {
                var startinfo = new ProcessStartInfo()
                {
                    UserName = "SYSTEM",
                    FileName = appFullPath
                };

                return Process.Start(startinfo);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static Process StartupAppAsLogonUser(string appFullPath)
        /// <summary>
        ///  startup app as logon user
        /// </summary>
        private static Process StartupAppAsLogonUser(string appFullPath)
        {
            try
            {
                var sessionid = ProcessApiHelp.GetCurrentUserSessionID();
                if (sessionid > 0)
                {
                    return ProcessApiHelp.CreateProcessAsUser(appFullPath, null);
                }
                else
                {
                    throw new Exception("HHITtoolsService.StartupAppAsLogonUser() fail: " + appFullPath);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void CloseOrKillProcess(Process process)
        private static void CloseOrKillProcess(Process process)
        {
            try
            {
                if (process != null)
                {
                    if (!process.HasExited)
                    {
                        process.CloseMainWindow();
                        if (!process.WaitForExit(_timeoutMillisecond))
                        {
                            process.Kill();
                            process.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + private static void CloseApp(string appFullPath)
        /// <summary>
        /// 強制 close app
        /// </summary>
        /// <param name="appFullPath"></param>
        private static void CloseApp(string appFullPath)
        {
            try
            {
                var appinfos = _AppProcessDictionary.Keys.Where(k => k.AppFullPath == appFullPath);
                foreach (var app in appinfos)
                {
                    if (_AppProcessDictionary.TryGetValue(app,out Process process))
                    {
                        CloseOrKillProcess(process);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // HHITtoolsUSB

        #region + public static void Startup_HHITtoolsUSB()
        public static void Startup_HHITtoolsUSB()
        {
            try
            {
                string appName = AgentRegistry.HHITtoolsUSBApp;

                var appinfo = _AppProcessDictionary.Keys.First(k => k.AppFullPath == appName);
                if (appinfo != null)
                {
                    if (_AppProcessDictionary.TryGetValue(appinfo,out Process process))
                    {
                        CloseOrKillProcess(process);
                    }
                }

                var newProc = StartupAppAsSystem(AgentRegistry.HHITtoolsUSBApp);
                var newAppInfo = new AppProcessInfo
                {
                    AppFullPath = appName,
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
            try
            {
                string appName = AgentRegistry.HHITtoolsTrayApp;
                var proc = StartupAppAsLogonUser(appName);

                AppProcessInfo appinfo = new AppProcessInfo
                {
                    AppFullPath = appName,
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
            AppManager_Entity.PrintJobNotify.Stop();
        }
        #endregion

    }
}
