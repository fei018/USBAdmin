using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBCommon
{
    public interface IIPPrinterInfo
    {
        /// <summary>
        /// Printer Driver Name
        /// </summary>
        string DriverName { get; set; }

        /// <summary>
        /// Printer Driver INF File UNC Path
        /// </summary>
        string DriverINFPath { get; set; }

        /// <summary>
        /// Printer Name
        /// </summary>
        string PrinterName { get; set; }

        /// <summary>
        /// Printer TCPIP Port Address
        /// </summary>
        string PortIPAddr { get; set; }
    }
}
