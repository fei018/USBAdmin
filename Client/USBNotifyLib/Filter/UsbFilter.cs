using System;

namespace USBNotifyLib
{
    public partial class UsbFilter
    {
        private readonly UsbBusController _usbBus;

        public UsbFilter()
        {
            _usbBus = new UsbBusController();
        }

        // Filter usb

        #region + public void Filter_UsbDisk_By_DriveLetter(char driveLetter)
        public void Filter_UsbDisk_By_DriveLetter(char driveLetter)
        {
            try
            {
                var usb = Get_UsbDisk_DiskPath_by_DriveLetter_WMI(driveLetter);
                if (usb != null)
                {
                    Filter_UsbDisk_By_DiskPath(usb);
                }
                else
                {
                    throw new Exception("Cannot find disk by driveLetter: " + driveLetter);
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        #region + public viod Filter_UsbDisk_By_DiskPath(UsbDisk usb)
        /// <summary>
        /// 只需 notifyUsb.DiskPath 賦值
        /// </summary>
        /// <param name="notifyUsb"></param>
        public void Filter_UsbDisk_By_DiskPath(UsbDisk usb)
        {
            try
            {
                if (!Find_UsbDeviceId_By_DiskPath_SetupDi(usb))
                {
                    Rule_NotFound_UsbDeviceID_By_DiskPath_SetupDi(usb);
                    return;
                }

                if (!_usbBus.Find_NotifyUSB_Detail_In_UsbBus(usb))
                {
                    Rule_NotFound_UsbDisk_Detail_In_UsbBus(usb);
                    return;
                }

                if (UsbWhitelistHelp.IsFind(usb))
                {
                    Rule_Match_In_UsbFilterData(usb);
                    return;
                }
                else
                {
                    Rule_NotMatch_In_UsbFilterData(usb);
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.ToString());
            }
        }
        #endregion

        #region + public void Filter_Scan_All_USB_Disk()
        /// <summary>
        /// 重新全局 scan usb disk to filter
        /// </summary>
        public void Filter_Scan_All_USB_Disk()
        {
            try
            {
                var usbList = Get_All_UsbDisk_DiskPath_by_BusType_USB_WMI();
                if (usbList.Count > 0)
                {
                    foreach (var usb in usbList)
                    {
                        Filter_UsbDisk_By_DiskPath(usb);
                    }
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        //Find usb

        #region + public UsbDisk Find_UsbDisk_By_DriveLetter(char driveLetter)
        public UsbDisk Find_UsbDisk_By_DriveLetter(char driveLetter)
        {
            try
            {
                var usb = Get_UsbDisk_DiskPath_by_DriveLetter_WMI(driveLetter);
                if (usb != null)
                {
                    if (Find_UsbDeviceId_By_DiskPath_SetupDi(usb))
                    {
                        if (_usbBus.Find_NotifyUSB_Detail_In_UsbBus(usb))
                        {
                            return usb;
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public UsbDisk Find_UsbDisk_By_DiskPath(string diskPath)
        public UsbDisk Find_UsbDisk_By_DiskPath(string diskPath)
        {
            try
            {
                var usb = new UsbDisk { DiskPath = diskPath };
                if (Find_UsbDeviceId_By_DiskPath_SetupDi(usb))
                {
                    if (_usbBus.Find_NotifyUSB_Detail_In_UsbBus(usb))
                    {
                        return usb;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //* Filter Rule *//

        #region + private void Rule_NotFound_UsbDeviceID_By_DiskPath_SetupDi(UsbDisk usb)
        /// <summary>
        /// 找不到 大多數係 非 USB device
        /// </summary>
        /// <param name="usb"></param>
        private void Rule_NotFound_UsbDeviceID_By_DiskPath_SetupDi(UsbDisk usb)
        {
            AgentLogger.Log("Not Find In SetupDi Usb DeviceId:\r\n" + usb.ToString());
        }
        #endregion

        #region + private void Rule_NotFound_UsbDisk_Detail_In_UsbBus(UsbDisk usb)
        /// <summary>
        /// should not happen
        /// </summary>
        /// <param name="notifyUsb"></param>
        private void Rule_NotFound_UsbDisk_Detail_In_UsbBus(UsbDisk usb)
        {
            // should not happen
        }
        #endregion

        #region + private void Rule_Match_In_UsbFilterData(UsbDisk usb)
        private void Rule_Match_In_UsbFilterData(UsbDisk usb)
        {
            try
            {
                // set ReadOnly false
                Set_Disk_IsReadOnly_by_DiskPath_WMI(usb.DiskPath, false);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + private void Rule_NotMatch_In_UsbFilterDb(UsbDisk usb)
        private void Rule_NotMatch_In_UsbFilterData(UsbDisk usb)
        {
            try
            {
                // set readonly true
                Set_Disk_IsReadOnly_by_DiskPath_WMI(usb.DiskPath, true);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
