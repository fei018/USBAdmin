using System;
using System.Threading.Tasks;

namespace USBNotifyLib
{
    public class AgentManager
    {
        #region Startup()
        public static void Startup()
        {
            try
            {
                // update agent setting
                new AgentHttpHelp().GetAgentSetting_Http();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // update UsbWhitelist
                new AgentHttpHelp().GetUsbWhitelist_Http();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // 上載 本機資訊
                new AgentHttpHelp().PostPerComputer_Http();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // registry 讀取 UsbFilterEnable 設定
                if (AgentRegistry.UsbFilterEnabled)
                {
                    // 載入 UsbWhitelist cache
                    UsbWhitelistHelp.Reload_UsbWhitelist();
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                //Debugger.Break();
                // 掃描過濾所有 usb disk
                if (AgentRegistry.UsbFilterEnabled)
                {
                    new UsbFilter().Filter_Scan_All_USB_Disk();
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // 執行 agent 定時任務
                AgentTimer.ReloadTask();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // start printjob notify
                PrintJobNotify.Start();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }
        }
        #endregion

        #region CloseApp()
        public static void CloseApp()
        {
            try
            {
                AgentTimer.ClearTimerTask();
            }
            catch (Exception) { }

            try
            {
                PrintJobNotify.Stop();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        // for Agent app

        #region + public static void PostUsbHistoryToHttpServer()
        public static void PostUsbHistoryToHttpServer(string diskPath)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!AgentRegistry.UsbHistoryEnabled) return;

                    // post usb history to server
                    new AgentHttpHelp().PostPerUsbHistory_byDisk_Http(diskPath);
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.Message);
                }
            });
        }
        #endregion

        #region + public static void CheckUsbWhitelist_PluginUSB()
        public static void CheckUsbWhitelist_PluginUSB(string diskPath)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!AgentRegistry.UsbFilterEnabled) return;

                    // push usbmessage to agent tray pipe
                    var usb = new UsbFilter().Find_UsbDisk_By_DiskPath(diskPath);
                    if (!UsbWhitelistHelp.IsFind(usb))
                    {
                        PipeServerAgent.Entity_Agent.PushMsg_ToTray_UsbDiskNotInWhitelist(usb);
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.ToString());
                }
            });
        }
        #endregion

        #region + public static void FilterUsbDisk(string diskPath)
        public static void FilterUsbDisk(string diskPath)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!AgentRegistry.UsbFilterEnabled) return;

                    var disk = new UsbDisk { DiskPath = diskPath };
                    new UsbFilter().Filter_UsbDisk_By_DiskPath(disk);
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.Message);
                }
            });
        }
        #endregion

    }
}
