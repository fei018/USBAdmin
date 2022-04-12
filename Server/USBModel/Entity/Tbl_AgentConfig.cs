using SqlSugar;
using ToolsCommon;

namespace USBModel
{
    public class Tbl_AgentConfig : IAgentConfig
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }


        public string AgentVersion { get ; set ; }

    }
}
