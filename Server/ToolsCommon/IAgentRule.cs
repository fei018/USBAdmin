namespace ToolsCommon
{
    public interface IAgentRule
    {
        int AgentTimerMinute { get; set; }

        string AgentVersion { get; set; }

        bool UsbFilterEnabled { get; set; }

        bool PrintJobLogEnabled { get; set; }
    }
}
