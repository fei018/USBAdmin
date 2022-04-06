namespace ToolsCommon
{
    public interface ISitePrinter
    {
        /// <summary>
        /// Printer Driver Name
        /// </summary>
        string DriverName { get; set; }

        /// <summary>
        /// Printer Driver Inf File UNC Path
        /// </summary>
        string DriverInfPath { get; set; }

        /// <summary>
        /// Printer Name
        /// </summary>
        string PrinterName { get; set; }

        /// <summary>
        /// Printer TCPIP Port Address
        /// </summary>
        string IPAddress { get; set; }
    }
}
