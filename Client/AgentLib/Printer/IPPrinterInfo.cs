using HHITtoolsCommon;

namespace AgentLib
{
    public class IPPrinterInfo : IIPPrinterInfo
    {
        /// <summary>
        /// Printer Driver Name
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// Printer Driver INF File UNC Path
        /// </summary>
        public string DriverInfPath { get; set; }

        /// <summary>
        /// Printer Name
        /// </summary>
        public string PrinterName { get; set; }

        /// <summary>
        /// Printer TCPIP Port Address
        /// </summary>
        public string PortIPAddr { get; set; }

        public string DriverInfLocalPath { get; set; }
    }
}
