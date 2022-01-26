using System;

namespace USBNotifyLib
{
    public class UsbDisk : UsbBase
    {
        public string DiskDeviceId { get; set; }

        public string DiskPath { get; set; }

        public uint DiskNumber { get; set; }

        public string DriveLetter { get; set; }

        public override string ToString()
        {
            string s = "USB Details :" + Environment.NewLine +
                       Vid_Hex + " - " + Vid + Environment.NewLine +
                       Pid_Hex + " - " + Pid + Environment.NewLine +
                       "SerialNumber: " + SerialNumber + Environment.NewLine +
                       "Manufacturer: " + Manufacturer + Environment.NewLine +
                       "Product: " + Product + Environment.NewLine +
                       "DeviceDescription: " + DeviceDescription + Environment.NewLine +
                       "DeviceId: " + UsbDeviceId + Environment.NewLine + Environment.NewLine;

            return s;
        }

    }
}
