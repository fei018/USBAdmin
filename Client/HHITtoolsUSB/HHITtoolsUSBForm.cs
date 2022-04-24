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
        public static HHITtoolsUSBForm HHToolsUSBForm { get; private set; }

        public HHITtoolsUSBForm()
        {
            InitializeComponent();

            HHToolsUSBForm = this;

#if DEBUG
            this.ShowInTaskbar = true;
#endif

            AppManager.Start();

        }

        #region this.Closed()
        private void USBNofityAgentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // unregister usb notify
            Stop();

            AppManager.Stop();          
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
                    UsbHelp.CheckUsbRegister_PluginUSB(args.Name);

                    UsbHelp.PostUsbHistoryToHttpServer(args.Name);
                }
            }
        }
        #endregion
      

        // OnUsbVolume

    }
}
