using AgentLib;
using AgentLib.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsUSB
{
    public class UsbHelp
    {
        #region + public static void PostUsbHistoryToHttpServer()
        public static void PostUsbHistoryToHttpServer(string diskPath)
        {
            Task.Run(() =>
            {
                try
                {
                    // post usb history to server
                    new AgentHttpHelp().PostUsbLog_byDisk(diskPath);
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
                    // check usbwhiltelist to set readonly
                    new UsbFilter().Filter_UsbDisk_By_DiskPath(diskPath);

                    var usb = new UsbFilter().Find_USBInfo_FromUsbDisk_By_DiskPath(diskPath);
                    if (!UsbWhitelist.IsFind(usb))
                    {
                        // push usbmessage to tray pipe
                        AppService.NamedPipeClient.SendMsgToTray_USBDiskNoRegister(usb);
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.ToString());
                }
            });
        }
        #endregion

        #region + public static void UpdateUSBWhiltelist_And_FilterAllUSB()
        public static void UpdateUSBWhiltelist_And_FilterAllUSB()
        {
            Task.Run(()=>
            {
                try
                {
                    new AgentHttpHelp().UpdateUSBWhitelist();                    
                }
                catch (Exception)
                {
                }

                try
                {
                    new UsbFilter().Filter_Scan_All_USBDisk();
                }
                catch (Exception)
                {
                }
            });
        }
        #endregion
    }
}
