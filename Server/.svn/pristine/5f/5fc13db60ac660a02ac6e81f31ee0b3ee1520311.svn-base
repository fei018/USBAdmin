using SqlSugar;
using System;
using USBCommon;

namespace USBModel
{
    public class Tbl_UsbRequest: IUsbRequest
    {
        [SugarColumn(IsIdentity =true,IsPrimaryKey =true)]
        public int Id { get; set; }

        public int Vid { get; set; }

        public int Pid { get; set; }

        public string SerialNumber { get; set; }

        [SugarColumn(UniqueGroupNameList = new string[] { "usbid" })]
        public string UsbIdentity { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Manufacturer { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Product { get; set; }

        [SugarColumn(IsNullable = true)]
        public string DeviceDescription { get; set; }

        public DateTime RequestTime { get; set; }


        [SugarColumn(DefaultValue = "UnderReview")]
        public string RequestState { get; set; }

        public DateTime RequestStateChangeTime { get; set; }


        [SugarColumn(IsNullable = true)]
        public string RequestComputerIdentity { get; set; }

        [SugarColumn(IsNullable = true)]
        public string RequestUserEmail { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string RequestReason { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string RejectReason { get; set; }

        // IsIgnore

        [SugarColumn(IsIgnore = true)]
        public string Vid_Hex => "0x_" + Vid.ToString("X").PadLeft(4, '0');

        [SugarColumn(IsIgnore = true)]
        public string Pid_Hex => "0x_" + Pid.ToString("X").PadLeft(4, '0');

        [SugarColumn(IsIgnore = true)]
        public string RequestTimeString => RequestTime.ToString("G");

        [SugarColumn(IsIgnore = true)]
        public string RequestStateChangeTimeString => RequestStateChangeTime.ToString("G");

        public override string ToString()
        {
            return $"\r\nManufacturer: {Manufacturer}\r\nProduct: { Product}\r\nVid: {Vid_Hex}\r\nPid: {Pid_Hex}\r\nSerialNumber: {SerialNumber}\r\n";
        }

        
    }
}
