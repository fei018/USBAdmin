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
using AgentLib;

namespace HHITtoolsTray.USBQRCode
{
    /// <summary>
    /// USBQRCodeWin.xaml 的互動邏輯
    /// </summary>
    public partial class USBQRCodeWin : Window
    {
        public USBQRCodeWin()
        {
            InitializeComponent();

            ShowWinLocationRightBottom();
        }

        /// <summary>
        /// 右下角 顯示 Window
        /// </summary>
        private void ShowWinLocationRightBottom()
        {
            this.Top = SystemParameters.WorkArea.Bottom - this.Height;
            this.Left = SystemParameters.WorkArea.Right - this.Width;
        }

        #region + public void SetUSBInfo(UsbDisk usbDisk)
        public void SetUSBInfo(UsbDisk usbDisk)
        {
            txtBrand.Text = usbDisk.Manufacturer;
            txtProduct.Text = usbDisk.Product;
            txtVid.Text = usbDisk.Vid.ToString();
            txtPid.Text = usbDisk.Pid.ToString();
            txtSerial.Text = usbDisk.SerialNumber;

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Manufacturer: " + usbDisk.Manufacturer);
            //sb.AppendLine("Product: " + usbDisk.Product);
            //sb.AppendLine("Vid: " + usbDisk.Vid.ToString());
            //sb.AppendLine("Pid: " + usbDisk.Pid.ToString());
            //sb.AppendLine("SerialNumber: " + usbDisk.SerialNumber);

            string qrText = $"Manufacturer:{usbDisk.Manufacturer}|Product:{usbDisk.Product}|Vid:{usbDisk.Vid}|Pid:{usbDisk.Pid}|SerialNumber:{usbDisk.SerialNumber}";

            imgUSBQRCode.Source = USBQRCodeHelp.GenerateBitmapImage(qrText);
        }
        #endregion
    }
}
