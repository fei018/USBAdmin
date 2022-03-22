namespace AgentLib
{
    public enum PipeMsgType
    {
        Msg_TrayHandle = 10,
        UsbDiskNoRegister_NotifyTray_ServerForward,
        UsbDiskNoRegister_NotifyTray_TrayHandle,
        UpdateAgent_ServerHandle,
        UpdateSetting_ServerHandle,
        CloseHHITtoolsUSB_USBAppHandle,
        CloseHHITtoolsTray_TrayHandle,
        DeleteOldPrintersAndInstallDriver_ServerHandle,
        DeleteOldPrintersAndInstallDriverCompleted,
        PrintJobNotifyRestart
    }
}
