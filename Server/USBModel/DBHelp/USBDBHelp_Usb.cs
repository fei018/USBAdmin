using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace USBModel
{
    public partial class USBDBHelp
    {
        // UsbRequest

        #region + public async Task<int> UsbRequest_TotalCount()
        public async Task<int> UsbRequest_TotalCount()
        {
            try
            {
                var total = await _db.Queryable<Tbl_UsbRequest>().CountAsync();
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + public async Task<int> UsbRequest_TotalCount_ByState(string state)
        public async Task<int> UsbRequest_TotalCount_ByState(string state)
        {
            try
            {
                var total = await _db.Queryable<Tbl_UsbRequest>()
                                        .Where(u => u.RequestState == state)
                                        .CountAsync();
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_UsbRequest> UsbRequest_Insert(Tbl_UsbRequest usb)
        public async Task<Tbl_UsbRequest> UsbRequest_Insert(Tbl_UsbRequest usb)
        {
            try
            {
                var exist = await _db.Queryable<Tbl_UsbRequest>().FirstAsync(u => u.UsbIdentity == usb.UsbIdentity);
                if (exist != null)
                {
                    return exist;
                }

                usb.RequestState = UsbRequestStateType.UnderReview;
                usb.RequestStateChangeTime = DateTime.Now;

                var usbInDb = await _db.Insertable(usb).ExecuteReturnEntityAsync();
                return usbInDb;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert UsbRequest Error:\r\n" + ex.GetBaseException().Message);
            }
        }
        #endregion

        #region + public async Task<(int total, List<Tbl_UsbRegRequest> list)> UsbRequest_Get_All(int pageIdnex, int pageSize)
        public async Task<(int total, List<Tbl_UsbRequest> list)> UsbRequest_Get_All(int pageIdnex, int pageSize)
        {
            try
            {
                RefAsync<int> total = new RefAsync<int>();
                var query = await _db.Queryable<Tbl_UsbRequest>()
                                        .OrderBy(u => u.RequestStateChangeTime, OrderByType.Desc)
                                        .ToPageListAsync(pageIdnex, pageSize, total);

                if (query == null || query.Count <= 0)
                {
                    throw new Exception("Nothing UsbRegRequest in Database.");
                }

                return (total.Value, query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int total, List<Tbl_UsbRequest> list)> UsbRequest_Get_ByStateType(int pageIdnex, int pageSize, string stateType)
        public async Task<(int total, List<Tbl_UsbRequest> list)> UsbRequest_Get_ByStateType(int pageIdnex, int pageSize, string stateType)
        {
            try
            {
                RefAsync<int> total = new RefAsync<int>();
                var query = await _db.Queryable<Tbl_UsbRequest>()
                                        .Where(u => u.RequestState == stateType)
                                        .OrderBy(u => u.RequestStateChangeTime, OrderByType.Desc)
                                        .ToPageListAsync(pageIdnex, pageSize, total);

                if (query == null || query.Count <= 0)
                {
                    throw new Exception("Cannot find any Tbl_UsbRequest.");
                }

                return (total.Value, query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_UsbRequest> UsbRequest_Get_ById(int id)
        public async Task<Tbl_UsbRequest> UsbRequest_Get_ById(int id)
        {
            try
            {
                var query = await _db.Queryable<Tbl_UsbRequest>().InSingleAsync(id);
                if (query == null)
                {
                    throw new Exception("Cannot find the Tbl_UsbRegRequest, Id: " + id);
                }

                return query;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_UsbRequest> UsbRequest_ToApprove_ById(int id)
        public async Task<Tbl_UsbRequest> UsbRequest_ToApprove_ById(int id)
        {
            try
            {
                var usbRegRequest = await UsbRequest_Get_ById(id);

                // set Tbl_UsbRegRequest state is Approve
                usbRegRequest.RequestState = UsbRequestStateType.Approve;
                usbRegRequest.RequestStateChangeTime = DateTime.Now;
                await _db.Updateable(usbRegRequest).ExecuteCommandAsync();

                return usbRegRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_UsbRequest> UsbRequest_ToReject()
        public async Task<Tbl_UsbRequest> UsbRequest_ToReject(Tbl_UsbRequest usbRequest)
        {
            try
            {
                usbRequest.RequestState = UsbRequestStateType.Reject;
                usbRequest.RequestStateChangeTime = DateTime.Now;

                await _db.Updateable(usbRequest).ExecuteCommandAsync();

                return usbRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task UsbRequest_Delete_ById(int id)
        public async Task UsbRequest_Delete_ById(int id)
        {
            try
            {
                await _db.Deleteable<Tbl_UsbRequest>().In(u => u.Id, id).ExecuteCommandAsync();

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region + public async Task<UsbRequestVM> UsbRequestVM_Get_ById(int id)
        public async Task<UsbRequestVM> UsbRequestVM_Get_ById(int id)
        {
            try
            {
                var usb = await _db.Queryable<Tbl_UsbRequest>().InSingleAsync(id);
                if (usb == null)
                {
                    throw new Exception("Cannot find the Tbl_UsbRegRequest, Id: " + id);
                }

                Tbl_ComputerInfo com = null;
                try
                {
                    com = await ComputerInfo_Get_ByIdentity(usb.RequestComputerIdentity);
                }
                catch (Exception)
                {
                }

                var vm = new UsbRequestVM(usb, com);

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int total, List<UsbRequestVM> list)> UsbRequestVM_Get_ByStateType(int pageIdnex, int pageSize, string stateType)
        public async Task<(int total, List<UsbRequestVM> list)> UsbRequestVM_Get_ByStateType(int pageIdnex, int pageSize, string stateType)
        {
            try
            {
                RefAsync<int> total = new RefAsync<int>();
                var usbList = await _db.Queryable<Tbl_UsbRequest>()
                                        .LeftJoin<Tbl_ComputerInfo>((u, c) => u.RequestComputerIdentity == c.ComputerIdentity)
                                        .Where(u => u.RequestState == stateType)
                                        .OrderBy(u => u.RequestStateChangeTime, OrderByType.Desc)
                                        .Select((u, c) => new { usb = u, com = c })
                                        .ToPageListAsync(pageIdnex, pageSize, total);

                if (usbList == null || usbList.Count <= 0)
                {
                    throw new Exception("Cannot find any Tbl_UsbRequest.");
                }

                var vmlist = new List<UsbRequestVM>();
                foreach (var list in usbList)
                {
                    vmlist.Add(new UsbRequestVM(list.usb, list.com));
                }

                return (total.Value, vmlist);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // UsbLog

        #region + public async Task UsbLog_Insert(Tbl_UsbLog usb)
        public async Task UsbLog_Insert(Tbl_UsbLog usb)
        {
            try
            {
                await _db.Insertable(usb).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int totalCount, List<UsbHistoryDetail> list)> UsbLog_Get_VMList(int pageIndex, int pageSize)
        public async Task<(int totalCount, List<UsbLogVM> list)> UsbLog_Get_VMList(int pageIndex, int pageSize)
        {
            try
            {
                var total = new RefAsync<int>();

                var query = await _db.Queryable<Tbl_UsbLog>()
                                        .LeftJoin<Tbl_ComputerInfo>((h, c) => h.ComputerIdentity == c.ComputerIdentity)
                                        .OrderBy(h => h.PluginTime, OrderByType.Desc)
                                        .Select((h, c) => new { his = h, com = c })
                                        .ToPageListAsync(pageIndex, pageSize, total);

                if (query == null || query.Count <= 0)
                {
                    throw new Exception("Nothing UsbHistory or UserUsb in database.");
                }

                //var pageList = query.OrderByDescending(o=>o.his.PluginTime)
                //                    .Skip((pageIndex - 1) * pageSize)
                //                    .Take(pageSize)
                //                    .ToList();

                var usbList = new List<UsbLogVM>();
                foreach (var q in query)
                {
                    usbList.Add(new UsbLogVM(q.his, q.com));
                }
                return (total.Value, usbList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int totalCount, List<Tbl_UserUsbHistory> list)> UsbLog_Get_List_ByComputerIdentity(string computerIdentity,int pageIndex, int pageSize)
        public async Task<(int totalCount, List<Tbl_UsbLog> list)> UsbLog_Get_List_ByComputerIdentity(string computerIdentity, int pageIndex, int pageSize)
        {
            try
            {
                var total = new RefAsync<int>();

                var query = await _db.Queryable<Tbl_UsbLog>()
                                        .Where(h => h.ComputerIdentity == computerIdentity)
                                        .OrderBy(h => h.PluginTime, OrderByType.Desc)
                                        .ToPageListAsync(pageIndex, pageSize, total);

                if (query == null || query.Count <= 0)
                {
                    throw new Exception("Nothing UsbHistory in database.");
                }

                return (total.Value, query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
