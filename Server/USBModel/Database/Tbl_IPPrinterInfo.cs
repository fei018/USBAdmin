using SqlSugar;
using System;
using HHITtoolsCommon;

namespace USBModel
{
    public class Tbl_IPPrinterInfo : IIPPrinterInfo
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "char(36)")]
        public string SiteId { get; set; }

        public string DriverName { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar(max)")]
        public string DriverInfPath { get; set; }

        public string PrinterName { get; set; }

        public string PortIPAddr { get; set; }
    }
}
