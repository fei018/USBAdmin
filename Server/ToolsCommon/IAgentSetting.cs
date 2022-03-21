namespace ToolsCommon
{
    public interface IAgentSetting
    {
        int AgentTimerMinute { get; set; }

        string AgentVersion { get; set; }

        bool UsbFilterEnabled { get; set; }

        bool PrintJobHistoryEnabled { get; set; }
    }
}
