using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace USBModel
{
    public partial class USBDBHelp
    {

        // BitlockerInfo

        #region + public async Task<(int total, List<Tbl_BitlockerInfo> list)> BitLockerInfo_Get_List(int pageIndex, int pageSize)
        public async Task<(int total, List<Tbl_BitLockerInfo> list)> BitLockerInfo_Get_List(int pageIndex, int pageSize)
        {
            try
            {
                RefAsync<int> total = new RefAsync<int>();

                var query = await _db.Queryable<Tbl_BitLockerInfo>().ToPageListAsync(pageIndex, pageSize, total);
                if (query == null || query.Count <= 0)
                {
                    throw new Exception("BitlockerInfo is empty.");
                }

                return (total.Value, query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_BitlockerInfo> BitLockerInfo_Get_ByPasswordID(string recoveryPasswordID)
        public async Task<Tbl_BitLockerInfo> BitLockerInfo_Get_ByPasswordID(string recoveryPasswordID)
        {
            try
            {
                var query = await _db.Queryable<Tbl_BitLockerInfo>().FirstAsync(b => b.RecoveryPasswordID == recoveryPasswordID);
                if (query == null)
                {
                    throw new Exception("cannot find bitlockerInfo, RecoveryPasswordID: " + recoveryPasswordID);
                }

                return query;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
