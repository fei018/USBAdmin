using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using USBCommon;
using LoginUserManager;

namespace USBModel
{
    public class USBAdminDatabaseHelp
    {
        private readonly ISqlSugarClient _db;

        public USBAdminDatabaseHelp(string connString)
        {
            _db = GetSqlClient(connString);
        }

        // Db

        #region + private ISqlSugarClient GetSqlClient(string conn)
        private ISqlSugarClient GetSqlClient(string conn)
        {
            ISqlSugarClient client = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = conn,
                DbType = DbType.SqlServer,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            return client;
        }
        #endregion

        #region + public void TryCreateDatabase()
        public void TryCreateDatabase()
        {
            try
            {
                _db.DbMaintenance.CreateDatabase();

                _db.CodeFirst.SetStringDefaultLength(100).InitTables<LoginUser>();
                _db.CodeFirst.InitTables<LoginErrorCountLimit>();

                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_PerUsbHistory>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_PerComputer>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_AgentSetting>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_UsbRequest>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_EmailSetting>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_PrintTemplate>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_IPPrinterInfo>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_IPPrinterSite>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_PerPrintJob>();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_BitLockerInfo>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        // AgentSetting

        #region + public async Task<t_AgentSetting> Get_AgentSetting()
        public async Task<IAgentSetting> Get_AgentSetting()
        {
            try
            {
                var query = await _db.Queryable<Tbl_AgentSetting>().FirstAsync();
                if (query == null)
                {
                    throw new Exception("AgentSetting is null.");
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // UsbWhitelist

        #region + public async Task<string> UsbWhitelist_Get()
        public async Task<string> UsbWhitelist_Get()
        {
            try
            {
                var query = await _db.Queryable<Tbl_UsbRequest>()
                                    .Where(u => u.RequestState == UsbRequestStateType.Approve)
                                    .ToListAsync();

                if (query == null || query.Count <= 0)
                {
                    throw new Exception("USB Whitelist is empty in database.");
                }

                StringBuilder whitelist = new StringBuilder();

                // UsbIdentity encode to Base64                   
                foreach (var u in query)
                {
                    whitelist.AppendLine(UtilityTools.Base64Encode(u.UsbIdentity));
                }
                return whitelist.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // UsbRegistry

        #region UsbRegistry
        //#region + public async Task Insert_UsbRegistered(UsbInfo usb)
        //public async Task Insert_UsbRegistered(Tbl_UsbRegistered usb)
        //{
        //    try
        //    {
        //        var usbInDb = await _db.Queryable<Tbl_UsbRegistered>()
        //                             .Where(u => u.UsbIdentity == usb.UsbIdentity)
        //                             .FirstAsync();

        //        if (usbInDb == null)
        //        {
        //            var succeed = await _db.Insertable(usb).ExecuteCommandIdentityIntoEntityAsync();
        //            if (!succeed)
        //            {
        //                throw new Exception("UsbRegistered insert fail.");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#endregion

        //#region + public async Task<int> Get_RegisteredUsbCount()
        //public async Task<int> Get_RegisteredUsbCount()
        //{
        //    try
        //    {
        //        var query = await _db.Queryable<Tbl_UsbRegistered>().CountAsync();

        //        return query;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#endregion

        //#region + public async Task<(int total, List<Tbl_UsbRegistered> list)> Get_UsbRegisteredList(int pageIndex, int pageSize)
        //public async Task<(int total, List<Tbl_UsbRegistered> list)> Get_UsbRegisteredList(int pageIndex, int pageSize)
        //{
        //    try
        //    {
        //        RefAsync<int> total = new RefAsync<int>();
        //        var query = await _db.Queryable<Tbl_UsbRegistered>()
        //                                .OrderBy(u => u.Id, OrderByType.Desc)
        //                                .ToPageListAsync(pageIndex, pageSize, total);

        //        if (query == null || query.Count <= 0)
        //        {
        //            throw new Exception("Nothing UsbRegistered in Database.");
        //        }

        //        return (total.Value, query);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#endregion
        #endregion

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

                //// approve 的 USB save to Tbl_UsbRegistered
                //var usb = new Tbl_UsbRegistered
                //{
                //    DeviceDescription = usbRegRequest.DeviceDescription,
                //    Manufacturer = usbRegRequest.Manufacturer,
                //    Pid = usbRegRequest.Pid,
                //    Vid = usbRegRequest.Vid,
                //    Product = usbRegRequest.Product,
                //    SerialNumber = usbRegRequest.SerialNumber,
                //    UsbIdentity = usbRegRequest.UsbIdentity
                //};
                //await Insert_UsbRegistered(usb);
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

                Tbl_PerComputer com = null;
                try
                {
                    com = await PerComputer_Get_ByIdentity(usb.RequestComputerIdentity);
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
                                        .LeftJoin<Tbl_PerComputer>((u, c) => u.RequestComputerIdentity == c.ComputerIdentity)
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

        // UsbHistory

        #region + public async Task Insert_UsbHistory(UsbHistory usb)
        public async Task Insert_UsbHistory(Tbl_PerUsbHistory usb)
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

        #region + public async Task<(int totalCount, List<UsbHistoryDetail> list)> Get_UsbHistoryVMList(int pageIndex, int pageSize)
        public async Task<(int totalCount, List<PerUsbHistoryVM> list)> Get_UsbHistoryVMList(int pageIndex, int pageSize)
        {
            try
            {
                var total = new RefAsync<int>();

                var query = await _db.Queryable<Tbl_PerUsbHistory>()
                                        .LeftJoin<Tbl_PerComputer>((h, c) => h.ComputerIdentity == c.ComputerIdentity)
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

                var usbList = new List<PerUsbHistoryVM>();
                foreach (var q in query)
                {
                    usbList.Add(new PerUsbHistoryVM(q.his, q.com));
                }
                return (total.Value, usbList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int totalCount, List<Tbl_UserUsbHistory> list)> Get_UsbHistoryListByComputerIdentity(string computerIdentity,int pageIndex, int pageSize)
        public async Task<(int totalCount, List<Tbl_PerUsbHistory> list)> Get_UsbHistoryListByComputerIdentity(string computerIdentity, int pageIndex, int pageSize)
        {
            try
            {
                var total = new RefAsync<int>();

                var query = await _db.Queryable<Tbl_PerUsbHistory>()
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


        // PerComputer

        #region public async Task<int> PerComputer_Get_TotalCount()
        public async Task<int> PerComputer_Get_TotalCount()
        {
            try
            {
                return await _db.Queryable<Tbl_PerComputer>().CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region +  public async Task<Tbl_PerComputer> PerComputer_Get_ById(int id)
        public async Task<Tbl_PerComputer> PerComputer_Get_ById(int id)
        {
            try
            {
                var query = await _db.Queryable<Tbl_PerComputer>().InSingleAsync(id);
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
        public async Task<(int totalCount, List<Tbl_PerComputer> list)> PerComputer_Get_List(int index, int size)
        {
            try
            {
                var total = new RefAsync<int>();
                var query = await _db.Queryable<Tbl_PerComputer>()
                                        .OrderBy(c=>c.LastSeen, OrderByType.Desc)
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
        public async Task<Tbl_PerComputer> PerComputer_Get_ByIdentity(string computerIdentity)
        {
            try
            {
                var query = await _db.Queryable<Tbl_PerComputer>().Where(c => c.ComputerIdentity == computerIdentity).FirstAsync();
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
        public async Task PerComputer_InsertOrUpdate(Tbl_PerComputer com)
        {
            try
            {
                com.LastSeen = DateTime.Now;

                var queryCom = await _db.Queryable<Tbl_PerComputer>()
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
        public async Task PerComputer_Delete_ById(int id)
        {
            try
            {
                if (await _db.Deleteable<Tbl_PerComputer>().In(c => c.Id, id).ExecuteCommandHasChangeAsync())
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

        // AgentSetting

        #region + public async Task<Tbl_AgentSetting> AgentSetting_Get()
        public async Task<Tbl_AgentSetting> AgentSetting_Get()
        {
            try
            {
                var query = await _db.Queryable<Tbl_AgentSetting>().FirstAsync();

                if (query == null)
                {
                    throw new Exception("Cannot find Tbl_AgentSetting.");
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_AgentSetting> AgentSetting_Update(Tbl_AgentSetting setting)
        public async Task<Tbl_AgentSetting> AgentSetting_Update(Tbl_AgentSetting setting)
        {
            try
            {
                var isUpdate = await _db.Updateable(setting).ExecuteCommandHasChangeAsync();

                if (!isUpdate)
                {
                    throw new Exception("Tbl_AgentSetting update fail.");
                }

                return setting;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // EmailSetting

        #region + public async Task<Tbl_EmailSetting> EmailSetting_Get()
        public async Task<Tbl_EmailSetting> EmailSetting_Get()
        {
            try
            {
                var query = await _db.Queryable<Tbl_EmailSetting>().FirstAsync();
                if (query == null)
                {
                    throw new Exception("EmailSetting is null.");
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_EmailSetting> EmailSetting_Update(Tbl_EmailSetting email)
        public async Task<Tbl_EmailSetting> EmailSetting_Update(Tbl_EmailSetting email)
        {
            try
            {
                Tbl_EmailSetting email2 = null;

                if (email.Id <= 0)
                {
                    email2 = await _db.Insertable(email).ExecuteReturnEntityAsync();

                    if (email2 == null)
                    {
                        throw new Exception("EmailSetting insert fail.");
                    }
                }
                else
                {
                    var isUpdate = await _db.Updateable(email).ExecuteCommandHasChangeAsync();

                    if (!isUpdate)
                    {
                        throw new Exception("EmailSetting update fail.");
                    }
                }

                return email2;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // PrintTemplate

        #region + public async Task<(int total, List<Tbl_PrintTemplate> list)> PrintTemplate_Get_All(int index, int size)
        public async Task<(int total, List<Tbl_PrintTemplate> list)> PrintTemplate_Get_All(int index, int size)
        {
            try
            {
                RefAsync<int> total = new RefAsync<int>();
                var list = await _db.Queryable<Tbl_PrintTemplate>().ToPageListAsync(index, size, total);
                if (list == null || list.Count <= 0)
                {
                    throw new Exception("Tbl_PrintTemplate is empty.");
                }

                return (total.Value, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_PrintTemplate> PrintTemplate_Get_BySubnetAddr(string subnetAddr)
        public async Task<Tbl_PrintTemplate> PrintTemplate_Get_BySubnetAddr(string subnetAddr)
        {
            try
            {
                var query = await _db.Queryable<Tbl_PrintTemplate>().FirstAsync(p => p.SubnetAddr == subnetAddr);
                if (query == null)
                {
                    throw new Exception("Cannot find the PrintTemplate, Subnet Address: " + subnetAddr);
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_PrintTemplate> PrintTemplate_Get_ById(int id)
        public async Task<Tbl_PrintTemplate> PrintTemplate_Get_ById(int id)
        {
            try
            {
                var query = await _db.Queryable<Tbl_PrintTemplate>().InSingleAsync(id);
                if (query == null)
                {
                    throw new Exception("Cannot find the PrintTemplate, Id: " + id);
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task PrintTemplate_Insert(Tbl_PrintTemplate template)
        public async Task PrintTemplate_Insert(Tbl_PrintTemplate template)
        {
            try
            {
                var exist = await _db.Queryable<Tbl_PrintTemplate>().AnyAsync(t => t.SubnetAddr == template.SubnetAddr);
                if (exist)
                {
                    throw new Exception("PrintTemplate Subnet Address is existed. Subnet: " + template.SubnetAddr);
                }

                var isInsert = await _db.Insertable(template).ExecuteCommandIdentityIntoEntityAsync();
                if (!isInsert)
                {
                    throw new Exception("PrintTemplate insert fail.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task PrintTemplate_Update(Tbl_PrintTemplate template)
        public async Task PrintTemplate_Update(Tbl_PrintTemplate template)
        {
            try
            {
                await _db.Updateable(template).ExecuteCommandAsync();
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region public async Task PrinterTemplate_Delete_ById(int id)
        public async Task PrinterTemplate_Delete_ById(int id)
        {
            try
            {
                await _db.Deleteable<Tbl_PrintTemplate>().In(t => t.Id, id).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // IPPrinterSite

        #region + public async Task<List<Tbl_IPPrinterSite>> IPPrinterSite_Get_ALL()
        public async Task<List<Tbl_IPPrinterSite>> IPPrinterSite_Get_ALL()
        {
            try
            {
                var list = await _db.Queryable<Tbl_IPPrinterSite>()
                                    .OrderBy(s=>s.UpdateTime, OrderByType.Desc)
                                    .ToListAsync();

                if (list == null || list.Count <= 0)
                {
                    throw new Exception("Tbl_IPPrinterSite is empty.");
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<IPPrinterSiteVM> IPPrinterSiteVM_Get_BySiteId(Guid siteId)
        public async Task<IPPrinterSiteVM> IPPrinterSiteVM_Get_BySiteId(Guid siteId)
        {
            try
            {
                var site = await _db.Queryable<Tbl_IPPrinterSite>().InSingleAsync(siteId);

                if (site == null)
                {
                    throw new Exception("Tbl_IPPrinterSite is empty. Id: " + siteId);
                }

                var printers = await _db.Queryable<Tbl_IPPrinterInfo>()
                                           .Where(p => p.SiteId == site.Id.ToString())
                                           .ToListAsync();

                var siteVM = new IPPrinterSiteVM(site, printers);

                return siteVM;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_IPPrinterSite> IPPrinterSite_Get_ById(Guid id)
        public async Task<Tbl_IPPrinterSite> IPPrinterSite_Get_ById(Guid id)
        {
            try
            {
                var query = await _db.Queryable<Tbl_IPPrinterSite>().InSingleAsync(id);
                if (query == null)
                {
                    throw new Exception("Cannot find the Tbl_IPPrinterSite, Id: " + id);
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterSite_Insert(Tbl_IPPrinterSite site)
        public async Task IPPrinterSite_Insert(Tbl_IPPrinterSite site)
        {
            try
            {
                var existName = await _db.Queryable<Tbl_IPPrinterSite>().FirstAsync(s => s.SiteName == site.SiteName);
                if (existName != null)
                {
                    throw new Exception("IPPrinterSite is exist, name: " + site.SiteName);
                }

                site.Id = Guid.NewGuid();
                site.UpdateTime = DateTime.Now;

                var insert = await _db.Insertable(site).ExecuteCommandIdentityIntoEntityAsync();
                if (!insert)
                {
                    throw new Exception("Tbl_IPPrinterSite insert new fail.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterSite_Update(Tbl_IPPrinterSite site)
        public async Task IPPrinterSite_Update(Tbl_IPPrinterSite site)
        {
            try
            {
                if (site.Id == Guid.Empty)
                {
                    throw new Exception("Id is empty.");
                }

                site.UpdateTime = DateTime.Now;
                await _db.Updateable(site).ExecuteCommandAsync();
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterSite_Delete_ById(Guid siteId)
        public async Task IPPrinterSite_Delete_ById(Guid id)
        {
            try
            {                
                await _db.Deleteable<Tbl_IPPrinterSite>().In(t => t.Id, id).ExecuteCommandAsync();

                // delete ipprinter
                await _db.Deleteable<Tbl_IPPrinterInfo>().Where(p => p.SiteId == id.ToString()).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // IPPrinterInfo

        #region + public async Task<Tbl_IPPrinterInfo> IPPrinterInfo_Get_ById(int id)
        public async Task<Tbl_IPPrinterInfo> IPPrinterInfo_Get_ById(int id)
        {
            try
            {
                var printer = await _db.Queryable<Tbl_IPPrinterInfo>().InSingleAsync(id);
                return printer;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_IList_BySiteSubnetAddr(string subnetAddr)
        public async Task<List<IIPPrinterInfo>> IPPrinterInfo_Get_IList_BySiteSubnetAddr(string subnetAddr)
        {
            try
            {
                var site = await _db.Queryable<Tbl_IPPrinterSite>().FirstAsync(s => s.SubnetAddr == subnetAddr);
                if (site == null)
                {
                    throw new Exception("PrinterSite is empty, Subnet Address: " + subnetAddr);
                }

                var printers = await _db.Queryable<Tbl_IPPrinterInfo>().Where(p => p.SiteId == site.Id.ToString()).ToListAsync();
                if (printers == null || printers.Count <= 0)
                {
                    throw new Exception("Printers is empty, subnet: " + subnetAddr);
                }

                var list = new List<IIPPrinterInfo>();
                foreach (var p in printers)
                {
                    list.Add(p);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List()
        public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List()
        {
            try
            {
                var list = await _db.Queryable<Tbl_IPPrinterInfo>().ToListAsync();
                if (list == null || list.Count <= 0)
                {
                    throw new Exception("Tbl_IPPrinterInfo is empty.");
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List_BySiteId(string siteId)
        public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List_BySiteId(string siteId)
        {
            try
            {
                var list = await _db.Queryable<Tbl_IPPrinterInfo>()
                                    .Where(p=>p.SiteId == siteId)
                                    .ToListAsync();

                if (list == null || list.Count <= 0)
                {
                    throw new Exception("Tbl_IPPrinterInfo is empty.");
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterInfo_Insert(Tbl_IPPrinterInfo printer)
        public async Task IPPrinterInfo_Insert(Tbl_IPPrinterInfo printer)
        {
            try
            {
                var insert = await _db.Insertable(printer).ExecuteCommandIdentityIntoEntityAsync();
                if (!insert)
                {
                    throw new Exception("Tbl_IPPrinterInfo insert fail. name: " + printer.PrinterName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterInfo_Update(Tbl_IPPrinterInfo printer)
        public async Task IPPrinterInfo_Update(Tbl_IPPrinterInfo printer)
        {
            try
            {
                if (printer.Id <= 0)
                {
                    throw new Exception("update fail. Id <= 0, name: " + printer.PrinterName);
                }

                var update = await _db.Updateable(printer).ExecuteCommandHasChangeAsync();
                if (!update)
                {
                    throw new Exception("IPPrinterInfo update fail, name:" + printer.PrinterName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterInfo_Delete_ById(int id)
        public async Task IPPrinterInfo_Delete_ById(int id)
        {
            try
            {
                await _db.Deleteable<Tbl_IPPrinterInfo>().In(p => p.Id, id).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // PerPrintJob

        #region + public async Task PerPrintJob_Insert(Tbl_PerPrintJob printJob)
        public async Task PerPrintJob_Insert(Tbl_PerPrintJob printJob)
        {
            try
            {
                await _db.Insertable(printJob).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int total,List<Tbl_PerPrintJob> list)> PerPrintJob_Get_List_ByComputerIdentity(string comIdentity, int pageIndex, int size)
        public async Task<(int total,List<Tbl_PerPrintJob> list)> PerPrintJob_Get_List_ByComputerIdentity(string comIdentity, int pageIndex, int size)
        {
            try
            {
                RefAsync<int> total = new RefAsync<int>();
                var list = await _db.Queryable<Tbl_PerPrintJob>()
                                    .Where(p => p.ComputerIdentity == comIdentity)
                                    .OrderBy(p => p.PrintingTime, OrderByType.Desc)
                                    .ToPageListAsync(pageIndex, size, total);

                if (list == null || list.Count <= 0)
                {
                    throw new Exception("PrintJob is empty.");
                }

                return (total.Value,list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

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
