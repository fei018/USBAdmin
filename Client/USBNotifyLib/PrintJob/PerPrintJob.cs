﻿using System;
using USBCommon;

namespace USBNotifyLib
{
    public class PerPrintJob : IPrintJobInfo
    {
        public int JobId { get; set; }
        public string FileName { get; set; }

        public int FilePages { get; set; }

        public string UserName { get; set; }

        public string ComputerName { get; set; }

        public DateTime PrintingTime { get; set; }

        public string PrinterName { get; set; }

        public string ComputerIdentity { get; set; }
    }
}
