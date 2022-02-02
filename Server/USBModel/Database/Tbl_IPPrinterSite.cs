using SqlSugar;
using System;

namespace USBModel
{
    public class Tbl_IPPrinterSite
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }

        public string SiteName { get; set; }

        [SugarColumn(ColumnDataType = "varchar(15)")]
        public string SubnetAddr { get; set; }      

        [SugarColumn(IsNullable = true)]
        public DateTime UpdateTime { get; set; }
    }
}
