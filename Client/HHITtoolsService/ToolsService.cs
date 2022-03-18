using System.ServiceProcess;

namespace HHITtoolsService
{
    public partial class ToolsService : ServiceBase
    {
        public ToolsService()
        {
            InitializeComponent();
            CanHandleSessionChangeEvent = true;
        }

        protected override void OnStart(string[] args)
        {
            ToolsServiceHelp.Start_Service();
        }

        protected override void OnStop()
        {
            ToolsServiceHelp.Stop_Service();
        }

        #region + protected override void OnSessionChange(SessionChangeDescription changeDescription)
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);

            // user logon windows
            // startup Agent tray
            if (changeDescription.Reason == SessionChangeReason.SessionLogon)
            {
                _autoBootUsbAgentTray = true;
                StartProcess_AgentTray();
            }

            // user logoff windows
            // close Agent tray
            if (changeDescription.Reason == SessionChangeReason.SessionLogoff)
            {
                _autoBootUsbAgentTray = false;
                CloseProcess_AgentTray();
            }
        }
        #endregion

    }
}
