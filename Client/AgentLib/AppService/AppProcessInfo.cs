using AgentLib.Win32API;
using System;
using System.Diagnostics;

namespace AgentLib.AppService
{
    public class AppProcessInfo
    {
        private const int _timeoutMillisecond = 2000;


        public Process AppProcess { get; private set; }

        public string AppFullPath { get; private set; }


        #region + public void CloseOrKillProcess()
        public void CloseOrKillProcess()
        {
            if (AppProcess == null)
            {
                return;
            }

            try
            {
                if (!AppProcess.HasExited)
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public static Process StartupAppAsLogonUser(string appFullPath)
        /// <summary>
        ///  startup app as logon user
        /// </summary>
        public static AppProcessInfo StartupAppAsLogonUser(string appFullPath)
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
