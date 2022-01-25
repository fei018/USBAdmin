using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using USBCommon;

namespace USBNotifyLib
{
    public class PerComputer : IPerComputer
    {
        public string HostName { get; set; }

        public string Domain { get; set; }

        public string BiosSerial { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }

        public string NetwordAddress { get; set; }

        public string ComputerIdentity => (BiosSerial + MacAddress)?.ToLower();

        public bool UsbFilterEnabled { get; set; }

        public bool UsbHistoryEnabled { get; set; }

        public string AgentVersion { get; set; }

        public override string ToString()
        {
            return "HostName: " + HostName + "\r\n" +
                   "Domain: " + Domain + "\r\n" +
                   "BiosSerial: " + BiosSerial + "\r\n" +
                   "IPAddress: " + IPAddress + "\r\n" +
                   "MacAddress: " + MacAddress + "\r\n";
        }
    }
}
