using System;
using System.Threading.Tasks;
using USBNotifyLib;
using USBNotifyAgentTray.USBWindow;
using System.Diagnostics;
using System.IO;
using USBNotifyAgentTray.PrintWindow;
using System.Windows;

namespace USBNotifyAgentTray
{
    public class TrayIcon
    {

        private System.Windows.Forms.NotifyIcon _trayIcon;


        public static TrayIcon Entity { get; set; }

        #region + private void RemoveTrayIcon()
        public void RemoveTrayIcon()
        {
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                _trayIcon = null;
            }
        }
        #endregion

        #region + private void AddTrayIcon()
        public void AddTrayIcon()
        {
#if DEBUG
            //Debugger.Break();
#endif
            try
            {
                RemoveTrayIcon();

                _trayIcon = new System.Windows.Forms.NotifyIcon
                {
                    Icon = Properties.Resources.icon,
                    Text = "IT Support Tools"
                };

                _trayIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

                _trayIcon.ContextMenuStrip.Items.Add("Remote Support", null, RunRemoteSupportVNC_Click);
                _trayIcon.ContextMenuStrip.Items.Add("Set Printer", null, SetPrinter_Click);
                _trayIcon.ContextMenuStrip.Items.Add("Update Setting", null, UpdateSettingItem_Click);
                //_trayIcon.ContextMenuStrip.Items.Add("Update Agent", null, UpdateAgentItem_Click);           
                _trayIcon.ContextMenuStrip.Items.Add("About", null, AboutItem_Click);
                _trayIcon.ContextMenuStrip.Items.Add("");

                _trayIcon.Visible = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        // Tray Item Click

        #region UpdateSettingItem_Click
        private void UpdateSettingItem_Click(object sender, EventArgs e)
        {
            try
            {
                PipeClientTray.Entity_Tray.PushMsg_ToAgent_UpdateSetting();
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
                PipeClientTray.Entity_Tray.PushMsg_ToAgent_CheckAndUpdateAgent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion

        #region RunRemoteSupportVNC_Click
        private void RunRemoteSupportVNC_Click(object sender, EventArgs e)
        {
            try
            {
                var vnc = Path.Combine(AgentRegistry.RemoteSupportExe);
                Process.Start(vnc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region SetPrinter_Click
        public bool Item_SetPrinter_IsOpen { get; set; } = false;

        private void SetPrinter_Click(object sender, EventArgs e)
        {
            try
            {
                if (Item_SetPrinter_IsOpen)
                {
                    return;
                }

                App.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        var prnWin = new SetPrinterWin();
                        prnWin.Show();

                        Item_SetPrinter_IsOpen = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Set Printer");
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region AboutItem_Click
        public bool Item_About_IsOpen { get; set; } = false;
        private void AboutItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Item_About_IsOpen)
                {
                    return;
                }

                App.Current.Dispatcher.Invoke(new Action(()=>
                {
                    var about = new AboutWin();
                    about.txtAgentVersion.Text = AgentRegistry.AgentVersion;
                    about.Show();

                    Item_About_IsOpen = true;
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
