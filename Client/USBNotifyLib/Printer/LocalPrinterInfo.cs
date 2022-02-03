namespace USBNotifyLib
{
    public class LocalPrinterInfo
    {
        public string Name { get; set; }

        public bool Local { get; set; }

        public bool Network { get; set; }

        public string PortName { get; set; }

        public string ServerName { get; set; }

        public TCPIPPrinterPort TCPIPPort { get; set; }

        public string GetIP()
        {
            return TCPIPPort?.HostAddress;
        }

        public bool IsTCPIPPrinter()
        {
            //string ipRegx = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            if (TCPIPPort != null)
            {
                return true;
            }

            return false;
        }
    }
}
