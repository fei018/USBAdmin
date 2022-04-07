using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace USBModel
{
    public partial class USBDBHelp
    {

        // PerComputer

        #region public async Task<int> PerComputer_Get_TotalCount()
        public async Task<int> ComputerInfo_Get_TotalCount()
        {
            try
            {
                return await _db.Queryable<Tbl_ComputerInfo>().CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region +  public async Task<Tbl_ComputerInfo> PerComputer_Get_ById(int id)
        public async Task<Tbl_ComputerInfo> ComputerInfo_Get_ById(int id)
        {
            try
            {
                var query = await _db.Queryable<Tbl_ComputerInfo>().InSingleAsync(id);
                if (query == null)
                {
                    throw new Exception("Cannot find the computer, Id: " + id);
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int totalCount,List<Tbl_PerComputer> list)> PerComputer_Get_List(int index, int size)
        public async Task<(int totalCount, List<Tbl_ComputerInfo> list)> ComputerInfo_Get_List(int index, int size)
        {
            try
            {
                var total = new RefAsync<int>();
                var query = await _db.Queryable<Tbl_ComputerInfo>()
                                        .OrderBy(c => c.LastSeen, OrderByType.Desc)
                                        .ToPageListAsync(index, size, total);

                if (query == null || query.Count <= 0)
                {
                    throw new Exception("Cannot find any computers in database.");
                }

                return (total.Value, query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<List<Tbl_PerComputer>> PerComputer_Get_ByIdentity(string computerIdentity)
        public async Task<Tbl_ComputerInfo> ComputerInfo_Get_ByIdentity(string computerIdentity)
        {
            try
            {
                var query = await _db.Queryable<Tbl_ComputerInfo>().Where(c => c.ComputerIdentity == computerIdentity).FirstAsync();
                if (query == null)
                {
                    throw new Exception("This computer is not registered, Identity: " + computerIdentity);
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task PerComputer_InsertOrUpdate(Tbl_PerComputer com)
        public async Task ComputerInfo_InsertOrUpdate(Tbl_ComputerInfo com)
        {
            try
            {
                com.LastSeen = DateTime.Now;

                var queryCom = await _db.Queryable<Tbl_ComputerInfo>()
                                     .Where(c => c.ComputerIdentity == com.ComputerIdentity)
                                     .FirstAsync();

                if (queryCom == null)
                {

                    await _db.Insertable(com).ExecuteCommandAsync();
                }
                else
                {
                    com.Id = queryCom.Id;

                    await _db.Updateable(com).ExecuteCommandAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task PerComputer_Delete_ById(int id)
        public async Task ComputerInfo_Delete_ById(int id)
        {
            try
            {
                if (await _db.Deleteable<Tbl_ComputerInfo>().In(c => c.Id, id).ExecuteCommandHasChangeAsync())
                {
                    return;
                }
                else
                {
                    throw new Exception("Computer delete fail. Id: " + id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


    }
}
