namespace AgentLib
{
    public enum PipeMsgType
    {
        Msg_ServiceToTray = 10,
        UsbDiskNoRegister_NotifyTray_USBToService,
        UsbDiskNoRegister_NotifyTray_ServiceToTray,
        UpdateAgent,
        UpdateSetting,
        CloseHHITtoolsUSB,
        CloseHHITtoolsTray,
        DeleteOldPrintersAndInstallDriver,
        DeleteOldPrintersAndInstallDriverCompleted,
        PrintJobNotifyRestart
    }
}
