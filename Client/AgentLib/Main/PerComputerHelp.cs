using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using HHITtoolsCommon;

namespace AgentLib
{
    public class PerComputerHelp
    {
        #region + public static string GetComputerIdentity()
        public static string GetComputerIdentity()
        {
            try
            {
                var com = new PerComputer();
                SetNetworkInfo(com);
                SetBiosSerial(com);

                if (string.IsNullOrWhiteSpace(com.ComputerIdentity))
                {
                    throw new Exception("ComputerIdentity is null or empty.");
                }
                return com.ComputerIdentity;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public static PerComputer GetPerComputer()
        public static PerComputer GetPerComputer()
        {
            try
            {
                PerComputer userComputer = new PerComputer();
                userComputer.AgentVersion = AgentRegistry.AgentVersion;
                userComputer.UsbFilterEnabled = AgentRegistry.UsbFilterEnabled;
                userComputer.UsbHistoryEnabled = AgentRegistry.UsbHistoryEnabled;
                userComputer.HostName = IPGlobalProperties.GetIPGlobalProperties().HostName;
                userComputer.Domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
                SetNetworkInfo(userComputer);
                SetBiosSerial(userComputer);
                return userComputer;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + public static string GetSubnetAddr()
        public static string GetSubnetAddr()
        {
            try
            {
                var com = new PerComputer();
                SetNetworkInfo(com);
                return com.NetwordAddress;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public static void SetNetworkInfo()
        public static void SetNetworkInfo(PerComputer com)
        {
            var nic = NetworkInterface.GetAllNetworkInterfaces()
                                    .Where(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                                    .Where(n => n.OperationalStatus == OperationalStatus.Up)
                                    .FirstOrDefault();

            //如果 wire nic 搵唔到, 嘗試搵 wireless nic
            if (nic == null)
            {
                nic = NetworkInterface.GetAllNetworkInterfaces()
                                    .Where(n => n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                    .Where(n => n.OperationalStatus == OperationalStatus.Up)
                                    .FirstOrDefault();
            }

            if (nic == null) throw new Exception("Cannot find ip address");

            // Set MAC Address            
            com.MacAddress = GetMACAddress(nic);

            var ipV4 = nic.GetIPProperties().UnicastAddresses
                            .Where(n => n.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            .First();

            // set IP Address
            com.IPAddress = ipV4.Address.ToString();

            // IPv4Mask
            com.IPv4Mask = ipV4.IPv4Mask.ToString();

            // set NetwordAddress
            com.NetwordAddress = UtilityTools.GetNetworkAddress(ipV4.Address, ipV4.IPv4Mask).ToString();
        }
        #endregion

        #region + private static void SetBiosSerial()
        /// <summary>
        /// if Bios Serial is null, to get mainboard serial
        /// </summary>
        /// <param name="userComputer"></param>
        private static void SetBiosSerial(PerComputer userComputer)
        {
            using (ManagementObjectSearcher ComSerial = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS"))
            {
                using (var wmi = ComSerial.Get())
                {
                    foreach (var b in wmi)
                    {
                        userComputer.BiosSerial = Convert.ToString(b["SerialNumber"])?.Trim();
                        b?.Dispose();
                    }
                }
            }

            if (string.IsNullOrEmpty(userComputer.BiosSerial))
            {
                using (ManagementObjectSearcher ComSerial = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
                {
                    using (var wmi = ComSerial.Get())
                    {
                        foreach (var b in wmi)
                        {
                            userComputer.BiosSerial = Convert.ToString(b["SerialNumber"])?.Trim();
                            b?.Dispose();
                        }
                    }
                }
            }
        }
        #endregion

        #region + private static string GetMACAddress(NetworkInterface nic)
        private static string GetMACAddress(NetworkInterface nic)
        {
            StringBuilder mac = new StringBuilder();
            byte[] bytes = nic.GetPhysicalAddress().GetAddressBytes();
            for (int i = 0; i < bytes.Length; i++)
            {
                // Display the physical address in hexadecimal.
                mac.Append(bytes[i].ToString("X2"));
                // Insert a hyphen after each byte, unless we are at the end of the address.
                if (i != bytes.Length - 1) mac.Append("-");
            }
            return mac.ToString();
        }
        #endregion
        
    }
}
