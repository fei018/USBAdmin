using SqlSugar;

namespace LoginUserManager
{
    public class LoginErrorCountLimit
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(DefaultValue ="10")]
        public int Count { get; set; }
    }
}
