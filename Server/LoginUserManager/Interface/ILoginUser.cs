namespace LoginUserManager
{
    public interface ILoginUser
    {
        int Id { get; set; }

        string Name { get; set; }

        string Password { get; set; }

        string RoleName { get; set; }

        int? LoginErrorCount { get; set; }

        bool? AccountLocked { get; set; }
    }
}
