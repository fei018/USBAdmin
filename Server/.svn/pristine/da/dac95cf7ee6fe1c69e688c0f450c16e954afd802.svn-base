using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoginUserManager
{
    public class LoginUserService : ILoginUserService
    {
        private readonly ISimpleClient<LoginUser> _tblLoginUser;

        private readonly ISimpleClient<LoginErrorCountLimit> _tblErrorCountLimit;

        private readonly HttpContext _httpContext;

        private const string _UserNameOrPasswordError = "UserName or Password Error.";

        public LoginUserService(LoginUserDb db, IHttpContextAccessor httpContextAccessor)
        {
            _tblLoginUser = db.Tbl_LoginUser;
            _tblErrorCountLimit = db.Tbl_LoginAllowErrorCount;

            _httpContext = httpContextAccessor.HttpContext;
        }

        #region 登入
        public async Task<LoginResult> Login(string userName, string password)
        {
            var result = new LoginResult();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return result.ReturnError(_UserNameOrPasswordError);
            }

            try
            {
                //检查 login name
                var dbuser = await _tblLoginUser.AsQueryable().FirstAsync(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
                if (dbuser == null) return result.ReturnError(_UserNameOrPasswordError);

                //检查 锁定, true 表示锁定
                if (dbuser.AccountLocked)
                {
                    return result.ReturnError(_UserNameOrPasswordError);
                }

                //检查 login错误次数
                if (IsLoginErrorCountMoreThanLimit(dbuser))
                {
                    return result.ReturnError(_UserNameOrPasswordError);
                }

                // 检查 密码，错误 LoginErrorCoun +1
                if (IsAccountPasswordTrue(dbuser, password))
                {
                    await SignIn(dbuser);
                    return result.ReturnLoginUser(dbuser);
                }

                return result.ReturnError(_UserNameOrPasswordError);
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }

        private bool IsLoginErrorCountMoreThanLimit(LoginUser user)
        {
            var limit = GetLoginErrorCountLimit().Result.LoginErrorCountLimit.Count;

            if (user.LoginErrorCount > limit)
            {
                user.AccountLocked = true;
                _tblLoginUser.Update(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAccountPasswordTrue(LoginUser user, string pass)
        {
            //检查 密码
            if (user.Password == pass)
            {
                user.LoginErrorCount = 0;
                _tblLoginUser.Update(user);
                return true;
            }
            else
            {
                user.LoginErrorCount += 1;
                _tblLoginUser.Update(user);
                return false;
            }
        }

        private async Task SignIn(LoginUser user)
        {
            var claimIndentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIndentity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claimIndentity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            var principal = new ClaimsPrincipal(claimIndentity);

            //验证参数内容
            var authProperties = new AuthenticationProperties
            {
                //是否永久保存cookie
                IsPersistent = false,

                // 永久保存 true , 需設置過期時間 ExpiresUtc
                //ExpiresUtc
            };

            await _httpContext?.SignInAsync(principal, authProperties);
        }
        #endregion

        #region 登出
        public async Task Logout()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                await _httpContext?.SignOutAsync();
            }
        }
        #endregion

        #region Get LoginUser List
        public async Task<LoginResult> GetAllUser()
        {
            var result = new LoginResult();

            try
            {
                var all = await _tblLoginUser.AsQueryable().OrderBy(u => u.Id, OrderByType.Asc).ToListAsync();
                if (all != null && all.Any())
                {
                    return result.ReturnLoginUserList(all);
                }
                return result.ReturnError("Nothing User in database.");
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region 新建 LoginUser
        public async Task<LoginResult> CreateLoginUser(LoginUser newUser)
        {
            var result = new LoginResult();

            try
            {
                var exist = await _tblLoginUser.AsQueryable().AnyAsync(u => u.Name == newUser.Name);
                if (exist)
                {
                    return result.ReturnError("UserName Exist.");
                }

                var user = await _tblLoginUser.AsInsertable(newUser).ExecuteReturnEntityAsync();

                return result.ReturnLoginUser(user);
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region update LoginUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns>LoginUser</returns>
        public async Task<LoginResult> UpdateLoginUser(LoginUser newUser)
        {
            var result = new LoginResult();

            if (newUser.Id <= 0)
            {
                return result.ReturnError("id <= 0");
            }


            try
            {
                var user = await _tblLoginUser.GetByIdAsync(newUser.Id);
                if (user == null)
                {
                    return result.ReturnError("Update fail.");
                }

                if (string.IsNullOrWhiteSpace(newUser.Password))
                {
                    newUser.Password = user.Password;
                }

                var up = await _tblLoginUser.UpdateAsync(newUser);

                if (up)
                {
                    return result.ReturnLoginUser(newUser);
                }
                else
                {
                    return result.ReturnError("Update fail.");
                }
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region 更改 LoginUser RoleName
        public async Task<LoginResult> ChangeUserRoleById(int id, string role)
        {
            var result = new LoginResult();

            try
            {
                var user = await _tblLoginUser.GetByIdAsync(id);
                if (user == null)
                {
                    return result.ReturnError($"Id={id} User not exist");
                }

                user.Role = role;
                var up = await _tblLoginUser.UpdateAsync(user);
                if (up)
                {
                    return result.ReturnLoginUser(user);
                }

                return result.ReturnError($"LoginUser: {user.Name} change Role failed.");
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region 更改 LoginUser Password
        public async Task<LoginResult> ChangeUserPasswordById(int id, string password)
        {
            var result = new LoginResult();

            try
            {
                var user = await _tblLoginUser.GetByIdAsync(id);
                if (user == null)
                {
                    return result.ReturnError($"Id={id} User not exist.");
                }

                user.Password = password;
                var up = await _tblLoginUser.UpdateAsync(user);
                if (up)
                {
                    return result.ReturnLoginUser(user);
                }

                return result.ReturnError($"LoginUser: {user.Name} change password failed.");
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region Get LoginUser
        public async Task<LoginResult> GetUserById(int id)
        {
            var result = new LoginResult();

            try
            {
                var user = await _tblLoginUser.GetByIdAsync(id);

                if (user == null)
                {
                    return result.ReturnError($"Id={id} User not exist.");
                }

                return result.ReturnLoginUser(user);
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region Delete LoginUser
        public async Task<LoginResult> DeleteUserById(int id)
        {
            var result = new LoginResult();

            try
            {
                var del = await _tblLoginUser.DeleteByIdAsync(id);

                if (del)
                {
                    return result.ReturnSuccess($"Id={id} User deleted.");
                }

                return result.ReturnError($"Id={id} User delete fail.");
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region Set LoginErrorCountLimit
        public async Task<LoginResult> SetLoginErrorCountLimit(int count)
        {
            var result = new LoginResult();

            try
            {
                var query = await _tblErrorCountLimit.AsQueryable().FirstAsync();
                if (query != null)
                {
                    await _tblErrorCountLimit.DeleteAsync(query);
                }

                var allow = new LoginErrorCountLimit { Count = count };
                var set = await _tblErrorCountLimit.InsertAsync(allow);
                if (set)
                {
                    return result.ReturnLoginErrorCountLimit(allow);
                }
                return result.ReturnError("Set LoginErrorCountLimit fail.");
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion

        #region Get LoginErrorCountLimit, 错误返回 count = 10
        public async Task<LoginResult> GetLoginErrorCountLimit()
        {
            var result = new LoginResult();

            try
            {
                var first = await _tblErrorCountLimit.AsQueryable().FirstAsync();
                if (first != null)
                {
                    return result.ReturnLoginErrorCountLimit(first);
                }

                return result.ReturnLoginErrorCountLimit(new LoginErrorCountLimit { Count = 10 });
            }
            catch (Exception ex)
            {
                return result.ReturnError(ex.Message);
            }
        }
        #endregion
    }
}
