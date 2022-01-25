using SqlSugar;

namespace LoginUserManager
{
    public class LoginUserDb : ILoginUserDb
    {
        private readonly ISqlSugarClient _dbClient;

        public LoginUserDb(string connString)
        {
            _dbClient = GetDbClient(connString);
        }

        private ISqlSugarClient GetDbClient(string conn)
        {
            var client = new SqlSugarClient(new ConnectionConfig
            {
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                ConnectionString = conn
            });

            return client;
        }

        public ISimpleClient<LoginUser> Tbl_LoginUser => new SimpleClient<LoginUser>(_dbClient);

        public ISimpleClient<LoginErrorCountLimit> Tbl_LoginAllowErrorCount => new SimpleClient<LoginErrorCountLimit>(_dbClient);
    }
}
