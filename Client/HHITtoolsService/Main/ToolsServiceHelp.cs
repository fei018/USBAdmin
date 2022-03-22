using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using AgentLib;
using AgentLib.Win32API;

namespace HHITtoolsService
{
    public static class ToolsServiceHelp
    {
        #region ServiceStart()
        public static void Start_Service()
        {
            AppManager.Start();
        }
        #endregion

        #region ServiceStop()
        public static void Stop_Service()
        {
            AppManager.Stop();
        }
        #endregion

        #region + public static void OnSessionChange(SessionChangeDescription sessionChange)
        public static void OnSessionChange(SessionChangeDescription sessionChange)
        {
            // user logon windows
            // startup tray
            if (sessionChange.Reason == SessionChangeReason.SessionLogon)
            {
                AppManager.Startup_HHITtoolsTray();
            }

            // user logoff windows
            // close tray
            if (sessionChange.Reason == SessionChangeReason.SessionLogoff)
            {
                AppManager.Close_HHITtoolsTray();
            }
        }
        #endregion

    }
}
