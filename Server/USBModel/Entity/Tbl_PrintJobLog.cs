using SqlSugar;
using System;
using ToolsCommon;

namespace USBModel
{
    public class Tbl_PrintJobLog : IPrintJobLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar(255)")]
        public string FileName { get; set; }

        public int FilePages { get; set; }

        public string UserName { get; set; }

        public string ComputerName { get; set; }

        public DateTime PrintingTime { get; set; }

        public string PrinterName { get; set; }

        public string ComputerIdentity { get; set; }



        //IsIgnore

        [SugarColumn(IsIgnore = true)]
        public string PrintingTimeString => PrintingTime.ToString("G");
    }
}
