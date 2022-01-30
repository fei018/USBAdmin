using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBModel
{
    public class IPPrinterSiteVM : Tbl_IPPrinterSite
    {
        public List<Tbl_IPPrinterInfo> PrinterList { get; set; }

        public IPPrinterSiteVM(Tbl_IPPrinterSite site, List<Tbl_IPPrinterInfo> printerList)
        {
            Id = site.Id;
            SiteName = site.SiteName;
            SubnetAddr = site.SubnetAddr;

            PrinterList = printerList;
        }
    }
}
