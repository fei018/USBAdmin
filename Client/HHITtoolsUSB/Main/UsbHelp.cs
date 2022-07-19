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
        #region + public void PostUsbHistoryToHttpServer()
        public void PostUsbLogToHttpServer(string diskPath)
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
        }
        #endregion

        #region + public void CheckUsbWhitelist_Arrival()
        public void CheckUsbWhitelist_Arrival(string diskPath)
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
                AgentLogger.Error("HHITtoolsUSB.CheckUsbRegister_PluginUSB(): " + ex.Message);
            }
        }
        #endregion

        #region + public void DiskSetReadWrite(string diskPath)
        public void DiskSetReadWrite(string diskPath)
        {
            try
            {
                new UsbFilter().Set_Disk_IsReadOnly_by_DiskPath_WMI(diskPath, false);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("HHITtoolsUSB.DiskSetReadWrite(): " + ex.Message);
            }
        }
        #endregion

        // static

        #region + public static void UpdateUSBWhiltelist_And_FilterAllUSB()
        public static void UpdateUSBWhiltelist_And_FilterAllUSB()
        {
            Task.Factory.StartNew(()=>
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
                    new UsbFilter().Filter_ScanAll_USBDisk();
                }
                catch (Exception)
                {
                }
            });
        }
        #endregion

        #region + public static void USBDisk_Arrival(string diskPath)
        public static void USBDisk_Arrival(string diskPath)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (AgentRegistry.UsbFilterEnabled)
                    {
                        new UsbHelp().CheckUsbWhitelist_Arrival(diskPath);
                    }
                    else
                    {
                        new UsbHelp().DiskSetReadWrite(diskPath);
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    if (AgentRegistry.UsbLogEnabled)
                    {
                        new UsbHelp().PostUsbLogToHttpServer(diskPath);
                    }
                }
                catch (Exception)
                {
                }
            });
        }
        #endregion
    }
}
