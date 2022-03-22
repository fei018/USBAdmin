using AgentLib.Win32API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;

namespace HHITtoolsService
{
    public class AppProcessInfo
    {
        private const int _timeoutMillisecond = 2000;


        public Process AppProcess { get; set; }

        public string AppFullPath { get; set; }      

        #region + public bool ProcessExsit()
        public bool ProcessExsit()
        {
            if (AppProcess == null)
            {
                return false;
            }

            try
            {
                if (AppProcess.HasExited)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region + public void CloseOrKillProcess()
        public void CloseOrKillProcess()
        {
            try
            {
                if (ProcessExsit())
                {
                    AppProcess.CloseMainWindow();
                    if (!AppProcess.WaitForExit(_timeoutMillisecond))
                    {
                        AppProcess.Kill();
                        AppProcess.Close();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        // static function

        #region + public static AppProcessInfo StartupApp(string appFullPath, string userName=null)
        public static AppProcessInfo StartupApp(string appFullPath, string userName = null)
        {
            try
            {
                var startinfo = new ProcessStartInfo()
                {
                    UserName = userName,
                    FileName = appFullPath
                };

                var proc = Process.Start(startinfo);

                var appInfo = new AppProcessInfo
                {
                    AppFullPath = appFullPath,
                    AppProcess = proc
                };

                return appInfo;
            }
            catch (Exception ex)
            {
                AgentLogger.Error("AppProcessInfo.StartupApp(): " + ex.Message);
                return null;
            }
        }
        #endregion

        #region + public static Process StartupAppAsLogonUser(string appFullPath)
        /// <summary>
        ///  startup app as logon user
        /// </summary>
        public static AppProcessInfo StartupProcessAsLogonUser(string appFullPath)
        {
            try
            {
                Process proc = null;

                var sessionid = ProcessApiHelp.GetCurrentUserSessionID();
                if (sessionid > 0)
                {
                    proc = ProcessApiHelp.CreateProcessAsUser(appFullPath, null);
                }
                else
                {
                    throw new Exception("StartupProcessAsLogonUser() : ProcessApiHelp.GetCurrentUserSessionID() <= 0 , " + appFullPath);
                }

                var appinfo = new AppProcessInfo
                {
                    AppFullPath = appFullPath,
                    AppProcess = proc
                };

                return appinfo;
            }
            catch (Exception ex)
            {
                AgentLogger.Error("AppProcessInfo.StartupProcessAsLogonUser(): " + ex.Message);
                return null;
            }
        }
        #endregion
    }
}
