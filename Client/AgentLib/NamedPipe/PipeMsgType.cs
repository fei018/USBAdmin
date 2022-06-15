namespace AgentLib
{
    public enum PipeMsgType
    {
        Msg_TrayHandle = 10,
        UsbDiskNoRegister_NotifyTray_ServerForward,
        UsbDiskNoRegister_NotifyTray_TrayHandle,
        UpdateAgent_ServerHandle,
        UpdateSetting_ServerHandle,
        PrintJobLogRestart,
        ToCloseProcess_HHITtoolsTray_TrayHandle,
        ToCloseProcess_HHITtoolsUSB_USBHandle
    }
}
