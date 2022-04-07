using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToolsCommon;
using LoginUserManager;

namespace USBModel
{
    public partial class USBDBHelp
    {
        private readonly ISqlSugarClient _db;

        public USBDBHelp(string connString)
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

                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_UsbLog>();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_ComputerInfo>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_AgentSetting>();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_UsbRequest>();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_EmailSetting>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_IPPrinterInfo>();
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_IPPrinterSite>();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_PrintJobLog>();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_BitLockerInfo>();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables<Tbl_AgentRule>();
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

        // AgentRule
        #region + public async Task<Tbl_AgentRule> AgentRule_Get_By_ComputerIdentity(string computerIdentity)
        public async Task<Tbl_AgentRule> AgentRule_Get_By_ComputerIdentity(string computerIdentity)
        {
            try
            {
                var com = await _db.Queryable<Tbl_ComputerInfo>()
                             .FirstAsync(c => c.ComputerIdentity.Equals(computerIdentity, StringComparison.OrdinalIgnoreCase));

                if (com == null)
                {
                    throw new Exception("Get AgentRule fail, cannot find the ComputerIdentity: " + computerIdentity);
                }

                Guid groupid = Guid.Parse(com.AgentRuleGroupId);

                var rule = await _db.Queryable<Tbl_AgentRule>()
                            .FirstAsync(r => r.GroupId.Equals(groupid));

                if (rule == null)
                {
                    throw new Exception("Get AgentRule fail, cannot find the GroupId: " + groupid.ToString());
                }

                return rule;
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

    }
}
