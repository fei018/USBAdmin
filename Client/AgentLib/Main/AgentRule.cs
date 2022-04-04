using ToolsCommon;

namespace AgentLib
{
    public class AgentRule : IAgentRule
    {
        public int AgentTimerMinute { get; set; }

        public string AgentVersion { get; set; }

        public bool UsbFilterEnabled { get; set; }

        public bool PrintJobLogEnabled { get; set; }
    }
}
