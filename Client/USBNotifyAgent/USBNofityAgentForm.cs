using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsbMonitor;
using USBNotifyLib;

namespace USBNotifyAgent
{
    public partial class USBNofityAgentForm : UsbMonitorForm
    {
        public USBNofityAgentForm()
        {
            OpenAppOneOnly();

            InitializeComponent();

#if DEBUG
            this.ShowInTaskbar = true;
#endif

            PipeAgentStart();

            AgentManager.Startup();
        }

        #region OpenAppOneOnly()
        private const string _mutexGuid = "32956814-4b61-4bd0-9571-cb6905995f23";
        private void OpenAppOneOnly()
        {
            Mutex mutex = new Mutex(true, _mutexGuid, out bool flag);
            if (!flag)
            {
                Environment.Exit(1);
            }
        }
        #endregion

        #region this.Closed()
        private void USBNofityAgentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
#if DEBUG
            //Debugger.Break();
#endif

            base.Stop();

            PipeServerAgent.Entity_Agent.PushMsg_ToTray_CloseTray();

            PipeServerAgent.Entity_Agent.Stop();

            AgentManager.Stop();           
        }
        #endregion

        // AgentPipe
        #region AgentPipe

        private void PipeAgentStart()
        {
            PipeServerAgent.Entity_Agent = new PipeServerAgent();
            PipeServerAgent.Entity_Agent.CloseAgentAppEvent += (s, e) => { this.Close(); };
            PipeServerAgent.Entity_Agent.Start();
        }
        #endregion

        // OnUsbInterface

        #region + public override void OnUsbInterface(UsbEventDeviceInterfaceArgs args)
        public override void OnUsbInterface(UsbEventDeviceInterfaceArgs args)
        {
            if (args.Action == UsbDeviceChangeEvent.Arrival)
            {
                if (args.DeviceInterface == UsbMonitor.UsbDeviceInterface.Disk)
                {
                    AgentManager.FilterUsbDisk(args.Name);

                    AgentManager.CheckUsbWhitelist_PluginUSB(args.Name);

                    AgentManager.PostUsbHistoryToHttpServer(args.Name);
                }
            }
        }
        #endregion
      

        // OnUsbVolume

        #region + public override void OnUsbVolume(UsbEventVolumeArgs args)
        //public override void OnUsbVolume(UsbEventVolumeArgs args)
        //{
        //    if (args.Action == UsbDeviceChangeEvent.Arrival)
        //    {
        //        if (args.Flags == UsbVolumeFlags.Media)
        //        {
        //            foreach (char letter in args.Drives)
        //            {
        //                FilterUsbDisk(letter);
        //            }
        //        }
        //    }
        //}
        #endregion

        #region + private void FilterUsbDisk(char letter)
        //private void FilterUsbDisk(char letter)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            if (!AgentRegistry.UsbFilterEnabled) return;

        //            new UsbFilter().Filter_UsbDisk_By_DriveLetter(letter);
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    });
        //}
        #endregion

    }
}
