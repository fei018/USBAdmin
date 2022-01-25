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
using USBNotifyLib;

namespace USBNotifyAgentTray.USBWindow
{
    /// <summary>
    /// UsbRequestRFormPage.xaml 的互動邏輯
    /// </summary>
    public partial class UsbRequestRFormPage : Page
    {
        public UsbRequestRFormPage()
        {
            InitializeComponent();
        }

        public event EventHandler<UsbRequestSubmitEventArgs> SubmittedEvent;

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {           
            // 檢查 email address or Reason
            if (string.IsNullOrWhiteSpace(txtUserEmail.Text) || string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Email address or Request reason is empty.", "Error");
                return;
            }

            var select = comboBoxEmail.SelectedItem as ComboBoxItem;
            if (select == null)
            {
                MessageBox.Show("Email SelectedItem is Null","Error");
                return;
            }

            try
            {
                
                var email = txtUserEmail.Text.Trim() + select.Content;

                // Submit Event
                SubmittedEvent?.Invoke(null,
                                       new UsbRequestSubmitEventArgs
                                       {
                                           UserEmailAddress = email,
                                           RequestReason = txtReason.Text.Trim()
                                       });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            //// post UsbRegRequest to http server
            //try
            //{
            //    if (TrayPipe.UsbDiskInfo == null)
            //    {
            //        throw new Exception("TrayPipe.UsbDiskInfo");
            //    }

            //    var post = new UsbRequest(TrayPipe.UsbDiskInfo, email, TxtReason.Text?.Trim());

            //    new AgentHttpHelp().PostUsbRequest(post);

            //    RequestPage_SubmittedEvent?.Invoke(this, "Your request was submitted successfully.");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error");
            //}
        }
    }
}
