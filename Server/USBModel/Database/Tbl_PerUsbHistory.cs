using SqlSugar;
using System;
using USBCommon;

namespace USBModel
{
    public class Tbl_PerUsbHistory : IPerUsbHistory
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public uint Id { get; set; }

        public int Vid { get; set; }

        public int Pid { get; set; }

        public string SerialNumber { get; set; }


        [SugarColumn(IsNullable = true)]
        public string Manufacturer { get; set; }

        [SugarColumn(IsNullable = true)]
        public string DeviceDescription { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Product { get; set; }

        public string ComputerIdentity { get; set; }

        public DateTime PluginTime { get; set; }

        // not mapping
        [SugarColumn(IsIgnore = true)]
        public string Pid_Hex => "0x_" + Pid.ToString("X").PadLeft(4, '0');

        [SugarColumn(IsIgnore = true)]
        public string Vid_Hex => "0x_" + Vid.ToString("X").PadLeft(4, '0');

        [SugarColumn(IsIgnore = true)]
        public string PluginTimeString => PluginTime.ToString("yyyy-MM-dd HH:mm:ss");

        [SugarColumn(IsIgnore = true)]
        public string UsbIdentity => (Vid.ToString() + Pid.ToString() + SerialNumber).ToLower();
    }
}
