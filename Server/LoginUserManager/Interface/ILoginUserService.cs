using System.Threading.Tasks;

namespace LoginUserManager
{
    public interface ILoginUserService
    {
        Task<LoginResult> Login(string userName, string password);

        Task Logout();

        Task<LoginResult> GetAllUser();

        Task<LoginResult> CreateLoginUser(LoginUser newUser);

        Task<LoginResult> ChangeUserRoleById(int id, string role);

        Task<LoginResult> ChangeUserPasswordById(int id, string password);

        Task<LoginResult> GetUserById(int id);

        Task<LoginResult> DeleteUserById(int id);

        Task<LoginResult> SetLoginErrorCountLimit(int count);
    }
}
