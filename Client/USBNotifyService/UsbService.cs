using System.ServiceProcess;

namespace USBNotifyService
{
    public partial class UsbService : ServiceBase
    {
        public UsbService()
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
