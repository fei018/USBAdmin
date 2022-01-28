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

        public string AdminName { get; set; }

        /// <summary>
        /// email 之間用 ; 分割
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(max)")]
        public string AdminEmailAddr { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Account { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Password { get; set; }

        [SugarColumn(IsNullable = true)]
        public string NotifyUrl { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string ApproveText { get; set; }


        // IsIgnore

        #region + public List<string> GetAdminEmailAddressList()
        public List<string> GetAdminEmailAddressList()
        {
            if (string.IsNullOrWhiteSpace(AdminEmailAddr))
            {
                return null;
            }

            var list = AdminEmailAddr.Split(';');

            List<string> emails = new List<string>();
            if (list.Length > 0)
            {
                foreach (var l in list)
                {
                    emails.Add(l.Trim());
                }
            }
            else
            {
                emails.Add(AdminEmailAddr);
            }

            return emails;
        }
        #endregion

    }
}
