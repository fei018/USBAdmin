using SqlSugar;
using System;
using ToolsCommon;

namespace USBModel
{
    public class Tbl_AgentRule : IAgentRule
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        public Guid GroupId { get; set; }


        public int AgentTimerMinute { get; set; }

        public string AgentVersion { get ; set ; }

        public bool UsbFilterEnabled { get; set; }

        public bool PrintJobLogEnabled { get; set; }

    }
}
