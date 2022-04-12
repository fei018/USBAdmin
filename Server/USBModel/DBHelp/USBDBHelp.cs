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
                _db.CodeFirst.SetStringDefaultLength(100).InitTables<Tbl_AgentConfig>();
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

        #region SeedData()
        public void SeedData()
        {

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

                if (string.IsNullOrWhiteSpace(com.AgentRuleName))
                {
                    com.AgentRuleName = "default";
                }

                var rule = await _db.Queryable<Tbl_AgentRule>()
                            .FirstAsync(r => r.RuleName.Equals(com.AgentRuleName, StringComparison.OrdinalIgnoreCase));

                if (rule == null)
                {
                    throw new Exception("Get AgentRule fail, cannot find the AgentRuleName: " + com.AgentRuleName);
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


        // Tbl_AgentConfig

        #region + public async Task<Tbl_AgentConfig> AgentConfig_Get()
        public async Task<Tbl_AgentConfig> AgentConfig_Get()
        {
            try
            {
                var query = await _db.Queryable<Tbl_AgentConfig>().FirstAsync();

                if (query == null)
                {
                    throw new Exception("Tbl_AgentConfig is null.");
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_AgentConfig> AgentConfig_Update(Tbl_AgentConfig config)
        public async Task<Tbl_AgentConfig> AgentConfig_Update(Tbl_AgentConfig config)
        {
            try
            {
                var isUpdate = await _db.Updateable(config).ExecuteCommandHasChangeAsync();

                if (!isUpdate)
                {
                    throw new Exception("Tbl_AgentConfig update fail.");
                }

                return config;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
