namespace AgentLib
{
    public enum PipeMsgType
    {
        Msg_ServerToTray = 10,
        UsbDiskNoRegister,
        UpdateAgent,
        UpdateSetting,
        CloseHHITtoolsUSB,
        CloseHHITtoolsTray,
        DeleteOldPrintersAndInstallDriver,
        DeleteOldPrintersAndInstallDriverCompleted,
        PrintJobNotifyRestart
    }
}
