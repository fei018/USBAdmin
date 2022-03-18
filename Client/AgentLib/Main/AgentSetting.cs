using ToolsCommon;

namespace AgentLib
{
    public class AgentSetting : IAgentSetting
    {
        public int AgentTimerMinute { get; set; }

        public string AgentVersion { get; set; }

        public bool UsbFilterEnabled { get; set; }

        public bool UsbHistoryEnabled { get; set; }

        public bool PrintJobHistoryEnabled { get; set; }
    }
}
