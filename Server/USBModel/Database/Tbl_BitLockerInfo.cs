using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBModel
{
    public class Tbl_BitLockerInfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string RecoveryPasswordID { get; set; }

        public string RecoveryPassword { get; set; }

        [SugarColumn(IsNullable = true)]
        public string DistinguishedName { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Time { get; set; }        
    }
}
