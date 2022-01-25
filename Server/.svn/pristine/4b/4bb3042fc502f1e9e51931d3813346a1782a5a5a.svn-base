using System.Collections.Generic;

namespace LoginUserManager
{
    public class LoginResult
    {
        public bool IsSucceed { get; set; }

        public string Message { get; set; }

        public LoginUser LoginUser { get; set; }

        public List<LoginUser> LoginUserList { get; set; }

        public LoginErrorCountLimit LoginErrorCountLimit { get; set; }

        public LoginResult ReturnError(string error)
        {
            IsSucceed = false;
            Message = error;
            return this;
        }

        public LoginResult ReturnSuccess(string message)
        {
            IsSucceed = true;
            Message = message;
            return this;
        }

        public LoginResult ReturnLoginUser(LoginUser user)
        {
            IsSucceed = true;
            LoginUser = user;
            return this;
        }

        public LoginResult ReturnLoginUserList(List<LoginUser> users)
        {
            IsSucceed = true;
            LoginUserList = users;
            return this;
        }

        public LoginResult ReturnLoginErrorCountLimit(LoginErrorCountLimit count)
        {
            IsSucceed = true;
            LoginErrorCountLimit = count;
            return this;
        }
    }
}
