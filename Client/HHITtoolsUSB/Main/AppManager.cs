using AgentLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsUSB
{
    public class AppManager
    {
        #region Startup()
        public static void Startup()
        {
            AppManager_Entity.InitialEntity();

            AppManager_Entity.PipeClient_USB.Start();
        }
        #endregion

        #region Close()
        public static void Close()
        {
            AppManager_Entity.PipeClient_USB?.Stop();
        }
        #endregion

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

        #region + public static void CheckUsbRegister_PluginUSB()
        public static void CheckUsbRegister_PluginUSB(string diskPath)
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
                        AppManager_Entity.PipeClient_USB.SendMsgToTray_USBDiskNoRegister(usb);
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
