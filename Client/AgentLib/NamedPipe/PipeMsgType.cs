namespace AgentLib
{
    public enum PipeMsgType
    {
        Message = 10,
        UsbDiskNoRegister,
        UpdateAgent,
        UpdateSetting,
        CloseAgent,
        CloseTray,
        AddPrintTemplate,
        AddPrintTemplateCompleted,
        PrinterDeleteOldAndInstallDriver,
        PrinterDeleteOldAndInstallDriverCompleted,
        PrintJobNotifyRestart
    }
}
