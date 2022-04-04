using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using AgentLib;
using AgentLib.AppService;
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
            try
            {
                if (sessionChange.Reason == SessionChangeReason.SessionLogon)
                {
                    // HHITtoolsTray
                    HHITtoolsTrayService appTray = new HHITtoolsTrayService();
                    appTray.Start();
                    AppService.HHITtoolsTrayList.Add(appTray);
                }
            }
            catch (Exception)
            {
            }

            // user logoff windows
            // close tray
            try
            {
                if (sessionChange.Reason == SessionChangeReason.SessionLogoff)
                {
                    var tray = AppService.HHITtoolsTrayList.Find(a => a.AppProcess.SessionId == sessionChange.SessionId);
                    AppService.HHITtoolsTrayList.Remove(tray);

                    AppProcessHelp.CloseOrKillProcess(tray.AppProcess);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
