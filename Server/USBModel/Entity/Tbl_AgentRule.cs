using SqlSugar;
using System;
using ToolsCommon;

namespace USBModel
{
    public class Tbl_AgentRule : IAgentRule
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(UniqueGroupNameList = new string[] { "RuleName" })]
        public string RuleName { get; set; }

        // setting

        public int AgentTimerMinute { get; set; }

        public bool UsbFilterEnabled { get; set; }

        public bool PrintJobLogEnabled { get; set; }

        public bool UsbLogEnabled { get; set; }
    }
}
