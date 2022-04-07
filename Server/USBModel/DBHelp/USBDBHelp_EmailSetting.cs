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

    }
}
