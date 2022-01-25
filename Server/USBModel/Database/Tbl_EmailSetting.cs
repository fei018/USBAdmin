using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBModel
{
    public class Tbl_EmailSetting
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        public string Smtp { get; set; }

        public int Port { get; set; }

        public string FromName { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar(max)")]
        public string FromAddress { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Account { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Password { get; set; }

        [SugarColumn(IsNullable = true)]
        public string NotifyUrl { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string ApproveText { get; set; }


        // IsIgnore

        #region + public List<string> GetFromAddressList()
        public List<string> GetFromAddressList()
        {
            if (string.IsNullOrWhiteSpace(FromAddress))
            {
                return null;
            }

            var list = FromAddress.Split(';');

            List<string> froms = new List<string>();
            if (list.Length > 0)
            {
                foreach (var l in list)
                {
                    froms.Add(l.Trim());
                }
            }
            else
            {
                froms.Add(FromAddress);
            }

            return froms;
        }
        #endregion

    }
}
