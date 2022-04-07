using SqlSugar;
using System.Collections.Generic;

namespace USBModel
{
    public class Tbl_EmailSetting
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        public string Smtp { get; set; }

        public int Port { get; set; }

        public string AdminName { get; set; }


        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string AdminEmailAddr { get; set; }

        /// <summary>
        /// email 之間用 ; 分割
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(max)", IsNullable = true)]
        public string ForwardEmailAddrList { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Account { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Password { get; set; }

        [SugarColumn(IsNullable = true)]
        public string NotifyUrl { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string ApproveText { get; set; }


        // IsIgnore

        #region + public List<string> GetForwardEmailAddrList()
        public List<string> GetForwardEmailAddrList()
        {
            List<string> emails = new List<string>();

            if (string.IsNullOrWhiteSpace(ForwardEmailAddrList))
            {
                return emails;
            }


            if (ForwardEmailAddrList.Contains(';'))
            {
                var list = ForwardEmailAddrList.Split(';');
                foreach (var l in list)
                {
                    emails.Add(l.Trim());
                }
            }
            else
            {
                emails.Add(ForwardEmailAddrList);
            }

            return emails;
        }
        #endregion

    }
}
