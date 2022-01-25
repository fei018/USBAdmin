using SqlSugar;
using USBCommon;

namespace USBModel
{
    public class Tbl_AgentSetting : IAgentSetting
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        public int AgentTimerMinute { get; set; }

        public string AgentVersion { get ; set ; }

        public bool UsbFilterEnabled { get; set; }

        public bool UsbHistoryEnabled { get; set; }
    }
}
