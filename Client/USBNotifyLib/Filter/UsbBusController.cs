using NativeUsbLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace USBNotifyLib
{
    internal class UsbBusController
    {
        private static ConcurrentDictionary<string, UsbBase> _ConnectedUSBList = new ConcurrentDictionary<string, UsbBase>();

        #region + public bool Find_PluginUSB_Detail_In_UsbBus_By_USBDeviceId(UsbDisk pluginUsb)
        /// <summary>
        /// if found, set Vid, Pid, SerialNumber to notifyUsb
        /// </summary>
        /// <param name="pluginUsb"></param>
        /// <returns></returns>
        public bool Find_PluginUSB_Detail_In_UsbBus_By_USBDeviceId(UsbDisk pluginUsb)
        {
            try
            {
                // _ConnectedUSBList key 不包含 pluginUsb.UsbDeviceId, go to ScanUsbBus()
                if (_ConnectedUSBList == null || _ConnectedUSBList.IsEmpty || !_ConnectedUSBList.ContainsKey(pluginUsb.UsbDeviceId.ToLower()))
                {
                    if (!ScanUsbBus())
                    {
                        throw new Exception("Cannot find any usb device in USB Controller."); // should not happen
                    }
                }

                if (_ConnectedUSBList.TryGetValue(pluginUsb.UsbDeviceId.ToLower(), out UsbBase device))
                {
                    pluginUsb.Vid = device.Vid;
                    pluginUsb.Pid = device.Pid;
                    pluginUsb.SerialNumber = device.SerialNumber;
                    pluginUsb.UsbDevicePath = device.UsbDevicePath;
                    pluginUsb.Manufacturer = device.Manufacturer;
                    pluginUsb.Product = device.Product;
                    pluginUsb.DeviceDescription = device.DeviceDescription;

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + ScanUsbBus()
        /// <summary>
        /// Get All USB Devices from USB Bus while the path != null
        /// </summary>
        /// <returns></returns>
        private bool ScanUsbBus()
        {
            ClearConnectedUSBListMoreThan1000();

            if (_ConnectedUSBList == null)
            {
                _ConnectedUSBList = new ConcurrentDictionary<string, UsbBase>();
            }

            var usbBus = new UsbBus();
            try
            {
                foreach (UsbController controller in usbBus.Controller)
                {
                    if (controller != null)
                    {
                        foreach (UsbHub hub in controller.Hubs)
                        {
                            if (hub != null)
                            {
                                if (hub.ChildDevices.Any())
                                {
                                    RecursionUsb(hub.ChildDevices);
                                }
                            }
                        }
                    }
                }

                if (_ConnectedUSBList != null && _ConnectedUSBList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                usbBus.Dispose();
            }
        }

        /// <summary>
        /// 遞歸獲取所有 usb device 放入 _ConnectedUSBList , key: InstanceId
        /// </summary>
        /// <param name="childDevices"></param>
        /// <param name="deviceList"></param>
        private void RecursionUsb(IReadOnlyCollection<Device> childDevices)
        {
            foreach (var device in childDevices)
            {
                if (device != null)
                {
                    if (!device.IsHub && device.IsConnected && !string.IsNullOrEmpty(device.DevicePath))
                    {
                        if (!_ConnectedUSBList.ContainsKey(device.InstanceId.ToLower()))
                        {
                            var usb = new UsbBase()
                            {
                                DeviceDescription = device.DeviceDescription,
                                Manufacturer = device.Manufacturer,
                                Pid = device.DeviceDescriptor.idProduct,
                                Product = device.Product,
                                SerialNumber = device.SerialNumber,
                                Vid = device.DeviceDescriptor.idVendor,
                                UsbDevicePath = device.DevicePath,
                                UsbDeviceId = device.InstanceId.ToLower()
                            };

                            _ConnectedUSBList.TryAdd(usb.UsbDeviceId, usb);
                        }                        
                    }

                    if (device.ChildDevices != null && device.ChildDevices.Any())
                    {
                        RecursionUsb(device.ChildDevices);
                    }
                }
            }
        }
        #endregion

        #region ClearConnectedUSBListMoreThan1000()
        private void ClearConnectedUSBListMoreThan1000()
        {
            try
            {
                if (_ConnectedUSBList.Count > 1000)
                {
                    _ConnectedUSBList.Clear();
                }
            }
            catch (Exception)
            {
                _ConnectedUSBList = new ConcurrentDictionary<string, UsbBase>();
            }
        }
        #endregion
    }
}
