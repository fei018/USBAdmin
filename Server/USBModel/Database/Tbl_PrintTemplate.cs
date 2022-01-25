using SqlSugar;
using USBCommon;

namespace USBModel
{
    public class Tbl_PrintTemplate : IPrintTemplate
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        public string SiteName { get; set; }

        public string SubnetAddr { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar(max)")]
        public string FilePath { get; set; }

    }
}
