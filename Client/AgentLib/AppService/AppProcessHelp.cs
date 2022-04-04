using AgentLib.Win32API;
using System;
using System.Diagnostics;

namespace AgentLib.AppService
{
    public class AppProcessHelp
    {
        private const int _timeoutMillisecond = 2000;


        #region + public void CloseOrKillProcess(Process)
        public static void CloseOrKillProcess(Process process)
        {
            if (process == null)
            {
                return;
            }

            try
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
            catch (Exception)
            {
            }
        }
        #endregion

        // static function

        #region + public static Process StartupApp(string appFullPath, string userName=null)
        public static Process StartupApp(string appFullPath, string userName = null)
        {
            try
            {
                var startinfo = new ProcessStartInfo()
                {
                    UserName = userName,
                    FileName = appFullPath
                };

                var proc = Process.Start(startinfo);

                return proc;
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
        public static Process StartupAppAsLogonUser(string appFullPath)
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

                return proc;
            }
            catch (Exception)
            {
                throw;
            }
        }       
        #endregion
    }
}
