using System.Collections.Generic;

namespace USBNotifyLib
{
    public class SitePrinterToAddList
    {
        public List<IPPrinterInfo> PrinterList { get; set; }

        public List<IPPrinterInfo> DriverList { get; set; }
    }
}
