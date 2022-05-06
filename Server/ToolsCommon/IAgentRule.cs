namespace ToolsCommon
{
    public interface IAgentRule
    {
        int AgentTimerMinute { get; set; }

        bool UsbFilterEnabled { get; set; }

        bool PrintJobLogEnabled { get; set; }

        bool UsbLogEnabled { get; set; }
    }
}
