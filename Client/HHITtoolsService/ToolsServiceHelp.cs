using AgentLib.AppService;
using System;
using System.ServiceProcess;

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

        }
        #endregion

    }
}
