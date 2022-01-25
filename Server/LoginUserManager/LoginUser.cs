using SqlSugar;

namespace LoginUserManager
{
    public class LoginUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //通过特性设置主键和自增列 
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string Name { get; set; }

        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string Password { get; set; }

        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string Role { get; set; }

        [SugarColumn(DefaultValue ="0")]
        public int LoginErrorCount { get; set; }

        [SugarColumn(DefaultValue = "false")]
        public bool AccountLocked { get; set; }
    }
}
