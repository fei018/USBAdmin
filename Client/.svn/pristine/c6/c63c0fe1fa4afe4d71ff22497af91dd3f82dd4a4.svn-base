using System;
using USBCommon;

namespace USBNotifyLib
{
    public class PerUsbHistory : IPerUsbHistory
    {
        public int Vid { get; set; }
        public int Pid { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Product { get; set; }
        public string DeviceDescription { get; set; }

        public string UsbIdentity => (Vid.ToString() + Pid.ToString() + SerialNumber).ToLower();

        public string ComputerIdentity { get; set; }

        public DateTime PluginTime { get; set; }

        public string PluginTimeString => PluginTime.ToString("yyyy-MM-dd HH:mm:ss");

        public string Vid_Hex => "0x_" + Vid.ToString("X").PadLeft(4, '0');

        public string Pid_Hex => "0x_" + Vid.ToString("X").PadLeft(4, '0');

    }
}
