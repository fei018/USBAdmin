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
                    FilterUsbDisk(args.Name);

                    CheckUsbWhitelist_PluginUSB(args.Name);

                    PostUsbHistoryToHttpServer(args.Name);
                }
            }
        }
        #endregion

        #region + private void PostUsbHistoryToHttpServer()
        private void PostUsbHistoryToHttpServer(string diskPath)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!AgentRegistry.UsbHistoryEnabled) return;

                    // post usb history to server
                    new AgentHttpHelp().PostPerUsbHistory_byDisk_Http(diskPath);
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.Message);
                }
            });
        }
        #endregion

        #region + private void CheckUsbWhitelist_PluginUSB()
        private void CheckUsbWhitelist_PluginUSB(string diskPath)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!AgentRegistry.UsbFilterEnabled) return;

                    // push usbmessage to agent tray pipe
                    var usb = new UsbFilter().Find_UsbDisk_By_DiskPath(diskPath);
                    if (!UsbWhitelistHelp.IsFind(usb))
                    {
                        PipeServerAgent.Entity_Agent.PushMsg_ToTray_UsbDiskNotInWhitelist(usb);
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.ToString());
                }
            });
        }
        #endregion

        #region + private void FilterUsbDisk(string diskPath)
        private void FilterUsbDisk(string diskPath)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!AgentRegistry.UsbFilterEnabled) return;

                    var disk = new UsbDisk { DiskPath = diskPath };
                    new UsbFilter().Filter_UsbDisk_By_DiskPath(disk);
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.Message);
                }
            });
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
