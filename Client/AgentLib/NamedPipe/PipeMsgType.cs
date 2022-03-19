namespace AgentLib
{
    public enum PipeMsgType
    {
        Msg_ServerToTray = 10,
        UsbDiskNoRegister_USBToTray,
        UpdateAgent,
        UpdateSetting,
        CloseHHITtoolsUSB,
        CloseHHITtoolsTray,
        DeleteOldPrintersAndInstallDriver,
        DeleteOldPrintersAndInstallDriverCompleted,
        PrintJobNotifyRestart
    }
}
