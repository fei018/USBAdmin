using System;

namespace ToolsCommon
{
    public interface IPrintJobLog
    {
        string FileName { get; set; }

        int FilePages { get; set; }

        string UserName { get; set; }

        string ComputerName { get; set; }

        DateTime PrintingTime { get; set; }

        string PrinterName { get; set; }

        string ComputerIdentity { get; set; }
    }
}
