namespace AgentLib
{
    public enum PipeMsgType
    {
        Msg_TrayHandle = 10,
        UsbDiskNoRegister_NotifyTray_ServerForward,
        UsbDiskNoRegister_NotifyTray_TrayHandle,
        UpdateAgent_ServerHandle,
        UpdateSetting_ServerHandle,
        DeleteOldPrintersAndInstallDriver_ServerHandle,
        DeleteOldPrintersAndInstallDriverCompleted,
        PrintJobLogRestart
    }
}
