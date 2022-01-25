using System;
using USBCommon;

namespace USBNotifyLib
{
    public class UsbBase : IUsbInfo
    {
        public int Vid { get; set; }

        public string Vid_Hex => "0x_" + Vid.ToString("X").PadLeft(4, '0');

        public int Pid { get; set; }

        public string Pid_Hex => "0x_" + Pid.ToString("X").PadLeft(4, '0');

        public string SerialNumber { get; set; }

        public string Manufacturer { get; set; }

        public string Product { get; set; }

        public string DeviceDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Lowercase string</returns>
        public string UsbIdentity => (Vid.ToString() + Pid.ToString() + SerialNumber).ToLower();
    }
}
