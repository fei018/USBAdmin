using NativeUsbLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace USBNotifyLib
{
    internal class UsbBusController
    {
        private List<Device> _busUsbList;

        private UsbBus _usbBus;

        #region + ScanUsbBus()
        /// <summary>
        /// Get All USB Devices from USB Bus while the path != null
        /// </summary>
        /// <returns></returns>
        private bool ScanUsbBus()
        {
            DisposeUSB();

            _usbBus = new UsbBus();

            _busUsbList = new List<Device>();

            foreach (UsbController controller in _usbBus.Controller)
            {
                if (controller != null)
                {
                    foreach (UsbHub hub in controller.Hubs)
                    {
                        if (hub != null)
                        {
                            RecursionUsb(hub.ChildDevices, ref _busUsbList);
                        }
                    }
                }

            }

            if (_busUsbList != null && _busUsbList.Count > 0)
            {
                return true;
            }
            else
            {
                DisposeUSB();
                return false;
            }
        }

        /// <summary>
        /// 遞歸獲取所有 usb device
        /// </summary>
        /// <param name="childDevices"></param>
        /// <param name="deviceList"></param>
        private void RecursionUsb(IReadOnlyCollection<Device> childDevices, ref List<Device> deviceList)
        {
            foreach (var d in childDevices)
            {
                if (d != null)
                {
                    if (!d.IsHub && !string.IsNullOrEmpty(d.DevicePath))
                    {
                        deviceList.Add(d);
                    }

                    if (d.ChildDevices != null && d.ChildDevices.Any())
                    {
                        RecursionUsb(d.ChildDevices, ref deviceList);
                    }
                }
            }
        }
        #endregion

        #region + Find_VidPidSerial_In_UsbBus(ref NotifyUSB notifyUsb)
        /// <summary>
        /// if found, set Vid, Pid, SerialNumber to notifyUsb
        /// </summary>
        /// <param name="notifyUsb"></param>
        /// <returns></returns>
        public bool Find_NotifyUSB_Detail_In_UsbBus(UsbDisk notifyUsb)
        {
            try
            {
                if (!ScanUsbBus())
                {
                    throw new Exception("Cannot find any usb device in USB Controller."); // should not happen
                }

                foreach (Device d in _busUsbList)
                {
                    if (d.InstanceId.Equals(notifyUsb.UsbDeviceId, StringComparison.OrdinalIgnoreCase))
                    {
                        notifyUsb.Vid = d.DeviceDescriptor.idVendor;
                        notifyUsb.Pid = d.DeviceDescriptor.idProduct;
                        notifyUsb.SerialNumber = d.SerialNumber;
                        notifyUsb.UsbDevicePath = d.DevicePath;
                        notifyUsb.Manufacturer = d.Manufacturer;
                        notifyUsb.Product = d.Product;
                        notifyUsb.DeviceDescription = d.DeviceDescription;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DisposeUSB();
            }
        }
        #endregion

        #region DisposeUSB
        private void DisposeUSB()
        {
            try
            {
                _busUsbList?.ForEach(d =>
                {
                    d?.Dispose();
                });

                _usbBus?.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
