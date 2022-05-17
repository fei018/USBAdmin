using System;
using System.Threading.Tasks;
using AgentLib;
using HHITtoolsTray.USBWindow;
using System.Diagnostics;
using System.IO;
using System.Windows;
using AgentLib.AppService;

namespace HHITtoolsTray
{
    public class TrayIcon : IAppService
    {

        private System.Windows.Forms.NotifyIcon _trayIcon;

        #region + public void Stop()
        public void Stop()
        {
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                _trayIcon = null;
            }
        }
        #endregion

        #region + public void Start()
        public void Start()
        {
#if DEBUG
            //Debugger.Break();
#endif
            try
            {
                Stop();

                _trayIcon = new System.Windows.Forms.NotifyIcon
                {
                    Icon = Properties.Resources.icon,
                    Text = "IT Support Tools"
                };

                _trayIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

                _trayIcon.ContextMenuStrip.Items.Add("Remote Support", null, RunRemoteSupport_Click);
                //_trayIcon.ContextMenuStrip.Items.Add("Set Printer", null, SetPrinter_Click);
                _trayIcon.ContextMenuStrip.Items.Add("Update Setting", null, UpdateSettingItem_Click);         
                _trayIcon.ContextMenuStrip.Items.Add("About", null, AboutItem_Click);
                //_trayIcon.ContextMenuStrip.Items.Add("Close", null, CloseTrayItem_Click);
                _trayIcon.ContextMenuStrip.Items.Add("");

                _trayIcon.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
        #endregion

        // Tray Item Click

        #region UpdateSettingItem_Click
        private void UpdateSettingItem_Click(object sender, EventArgs e)
        {
            try
            {
                AppService.NamedPipeClient.SendMsg_UpdateSetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region UpdateAgentItem_Click
        private void UpdateAgentItem_Click(object sender, EventArgs e)
        {
            try
            {
                AppService.NamedPipeClient.SendMsg_CheckAndUpdateAgent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion

        #region RunRemoteSupport_Click
        private void RunRemoteSupport_Click(object sender, EventArgs e)
        {
            try
            {
                var meshAgent = Path.Combine(AgentRegistry.HHITtoolsMeshAgentNotifyPath);
                Process.Start(meshAgent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region SetPrinter_Click
        private void SetPrinter_Click(object sender, EventArgs e)
        {
            try
            {
                if (WinSingleOpen.SetPrinterWin)
                {
                    return;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region AboutItem_Click
        private void AboutItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (WinSingleOpen.AboutWin)
                {
                    return;
                }

                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var about = new AboutWin();
                    about.txtAgentVersion.Text = AgentRegistry.AgentVersion;
                    about.Show();

                    WinSingleOpen.AboutWin = true;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region CloseTrayItem_Click
        private void CloseTrayItem_Click(object sender, EventArgs e)
        {
            try
            {
                App.Current.Dispatcher.Invoke(new Action(()=>{
                    App.Current.MainWindow.Close();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
