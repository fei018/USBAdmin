using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using USBNotifyLib;

namespace USBNotifyAgentTray.USBWindow
{
    /// <summary>
    /// UsbRequestWin.xaml 的互動邏輯
    /// </summary>
    public partial class UsbRequestWin : Window
    {
        public UsbRequestWin()
        {
            InitializeComponent();

            ShowWinLocationRightBottom();
        }

        private UsbDisk _UsbDiskInfo { get; set; }

        /// <summary>
        /// 右下角 顯示 Window
        /// </summary>
        private void ShowWinLocationRightBottom()
        {
            this.Top = SystemParameters.WorkArea.Bottom - this.Height;
            this.Left = SystemParameters.WorkArea.Right - this.Width;
        }

        /// <summary>
        /// 顯示 UsbRequestNotify Page
        /// </summary>
        public void ShowPageUsbRequestNotify(UsbDisk usbDisk)
        {
            try
            {
                _UsbDiskInfo = usbDisk;

                var notifyPage = new UsbRequestNotifyPage();
                if (_UsbDiskInfo == null)
                {
                    throw new Exception("UsbDiskInfo is null");
                }

                notifyPage.txtBrand.Text = _UsbDiskInfo.Manufacturer;
                notifyPage.txtProduct.Text = _UsbDiskInfo.Product;
                notifyPage.txtSerial.Text = _UsbDiskInfo.SerialNumber;

                notifyPage.ShowPageUsbRequestRFormEvent += NotifyPage_ShowPageUsbRequestRFormEvent;              

                controlPage.Content = new Frame { Content = notifyPage };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UsbRequestWin");
            }
        }

        /// <summary>
        /// 顯示 UsbRequestRForm Page
        /// </summary>
        private void NotifyPage_ShowPageUsbRequestRFormEvent(object sender, EventArgs e)
        {
            var usbRFormPage = new UsbRequestRFormPage();
            usbRFormPage.SubmittedEvent += UsbRFormPage_SubmittedEvent;

            this.Height = 350;
            this.Width = 400;

            controlPage.Content = new Frame { Content = usbRFormPage };
        }

        /// <summary>
        /// 按下 UsbRequestRFormPage Submit Button, Post UsbRequest to Http Server
        /// </summary>
        private void UsbRFormPage_SubmittedEvent(object sender, UsbRequestSubmitEventArgs e)
        {
            try
            {
                var usbRequest = new UsbRequest(_UsbDiskInfo, e.UserEmailAddress, e.RequestReason);
                new AgentHttpHelp().PostUsbRequest_Http(usbRequest);

                // 顯示 UsbRequestSubmitResult Page
                var usbRequestResultPage = new UsbRequestSubmitResultPage();
                usbRequestResultPage.txtSubmitResult.Text = "Your request was submitted successfully.";
                controlPage.Content = new Frame { Content = usbRequestResultPage };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
