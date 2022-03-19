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

            ToolsServiceHelp.OnSessionChange(changeDescription);
        }
        #endregion

    }
}
