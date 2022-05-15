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
        private MeshAgentServiceHelp _agentServiceHelp;

        /// <summary>
        /// 右下角 顯示 Window
        /// </summary>
        private void ShowWinLocationRightBottom()
        {
            this.Top = SystemParameters.WorkArea.Bottom - this.Height;
            this.Left = SystemParameters.WorkArea.Right - this.Width;
        }

        public MainWindow()
        {
            InitializeComponent();

            ShowWinLocationRightBottom();

            _agentServiceHelp = new MeshAgentServiceHelp();
            _agentServiceHelp.AgentServiceStateChangeEvent += _agentServiceHelp_AgentServiceStateChangeEvent;
            _agentServiceHelp.Start();
        }

        #region + private void _agentServiceHelp_AgentServiceStateChangeEvent(object sender, MeshAgentServiceState e)
        private void _agentServiceHelp_AgentServiceStateChangeEvent(object sender, MeshAgentServiceState e)
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

        private void Window_Closed(object sender, EventArgs e)
        {
            _agentServiceHelp.Stop();
        }

        private void btnAgentService_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (btn != null)
            {
                if(btn.Content.ToString() == "Start")
                {
                    // send msg to stop service
                }
                else
                {
                    // send msg to start service
                }

            }
        }

    }
}
