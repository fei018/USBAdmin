using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsbMonitor;

namespace HHITtoolsUSB
{
    public partial class HHITtoolsUSBForm : UsbMonitorForm
    {
        public HHITtoolsUSBForm()
        {
            InitializeComponent();

#if DEBUG
            this.ShowInTaskbar = true;
#endif

            AppManager_Entity.PipeClient_USB.ToCloseHHITtoolsUSBEvent += PipeClient_USB_ToCloseHHITtoolsUSBEvent;
        }

        private void PipeClient_USB_ToCloseHHITtoolsUSBEvent(object sender, EventArgs e)
        {
            this.Close();
        }

        #region this.Closed()
        private void USBNofityAgentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // unregister usb notify
            Stop();

            AppManager.Close();          
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
                    UsbHelp.FilterUsbDisk(args.Name);

                    UsbHelp.CheckUsbRegister_PluginUSB(args.Name);

                    UsbHelp.PostUsbHistoryToHttpServer(args.Name);
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
