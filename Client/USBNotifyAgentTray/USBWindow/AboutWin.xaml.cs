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

namespace USBNotifyAgentTray.USBWindow
{
    /// <summary>
    /// AboutWin.xaml 的互動邏輯
    /// </summary>
    public partial class AboutWin : Window
    {
        public AboutWin()
        {
            InitializeComponent();
        }

        private void btnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PipeClientTray.Entity_Tray?.PushMsg_ToAgent_CheckAndUpdateAgent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void AboutWin_Closed(object sender, EventArgs e)
        {
            TrayIcon.Entity.Item_About_IsOpen = false;
        }
    }
}
