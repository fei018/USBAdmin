using AgentLib;
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

namespace HHITtoolsMeshAgentNotify
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ShowWinLocationRightBottom();

            AppManger.Start();

            AppService.MeshAgentServiceHelp.AgentServiceStateChangeEvent += AgentServiceStateChangeEvent;
            AppService.MeshAgentServiceHelp.Start();
        }

        #region MyRegion
        /// <summary>
        /// 右下角 顯示 Window
        /// </summary>
        private void ShowWinLocationRightBottom()
        {
            this.Top = SystemParameters.WorkArea.Bottom - this.Height;
            this.Left = SystemParameters.WorkArea.Right - this.Width;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AppManger.Stop();
        }
        #endregion

        #region + private void AgentServiceStateChangeEvent(object sender, MeshAgentServiceState e)
        public void AgentServiceStateChangeEvent(object sender, MeshAgentServiceState e)
        {
            try
            {
                if (e == MeshAgentServiceState.NoService)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        txtbAgentServiceState.Text = "NoService";
                        btnAgentService.Content = "Start";
                    });
                    return;
                }

                if (e == MeshAgentServiceState.Running)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        txtbAgentServiceState.Text = "Running";
                        btnAgentService.Content = "Stop";
                    });
                    return;
                }

                if (e == MeshAgentServiceState.Stopped)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        txtbAgentServiceState.Text = "Stopped";
                        btnAgentService.Content = "Start";
                    });
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion      

        private async void btnAgentService_Click(object sender, RoutedEventArgs e)
        {
            DisableButton(true);

            try
            {
                if (e.Source is Button btn)
                {
                    if (btn.Content.ToString() == "Start")
                    {
                        // send msg to Start service
                        AppService.NamedPipeClient_MeshAgentNotify.SendMsg_ToStartMeshAgentService();
                    }
                    else
                    {
                        // send msg to Stop service
                        AppService.NamedPipeClient_MeshAgentNotify.SendMsg_ToStopMeshAgentService();
                    }

                    await Task.Delay(4000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            DisableButton(false);
        }

        #region + private void DisableButton(bool disbale)
        private void DisableButton(bool disbale)
        {
            try
            {
                btnAgentService.IsEnabled = !disbale;
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
