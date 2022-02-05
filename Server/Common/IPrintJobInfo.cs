using System;

namespace USBCommon
{
    public interface IPrintJobInfo
    {
        string FileName { get; set; }

        string FileSize { get; set; }

        string UserName { get; set; }

        string ComputerName { get; set; }

        DateTime PrintingTime { get; set; }

        string PrinterName { get; set; }

        string ComputerIdentity { get; set; }
    }
}
