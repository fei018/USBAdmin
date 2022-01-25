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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace USBNotifyAgentTray.USBWindow
{
    /// <summary>
    /// UsbRequestNotifyPage.xaml 的互動邏輯
    /// </summary>
    public partial class UsbRequestNotifyPage : Page
    {
        public UsbRequestNotifyPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按下 "USB registration request..." Button Event
        /// </summary>
        public event EventHandler ShowPageUsbRequestRFormEvent;

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            win.Close();
        }

        private void btnShowPageUsbRequestRForm_Click(object sender, RoutedEventArgs e)
        {
            ShowPageUsbRequestRFormEvent?.Invoke(null, null);
        }
    }
}
