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
            Start_Service();
        }

        protected override void OnStop()
        {
            Stop_Service();
        }

    }
}
