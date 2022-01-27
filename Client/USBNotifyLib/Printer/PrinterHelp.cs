using System;
using System.Collections.Generic;
using System.Management;
using System.Net;

namespace USBNotifyLib
{
    public class PrinterHelp
    {
        #region + private static List<TCPIPPrinterPort> GetTcpIPPrinterPortList()
        private static List<TCPIPPrinterPort> GetTcpIPPrinterPortList()
        {
            var tcpIPProtList = new List<TCPIPPrinterPort>();

            ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
            var query = new ObjectQuery("Select * from Win32_TCPIPPrinterPort");

            using (ManagementObjectSearcher objsearcher = new ManagementObjectSearcher(mgmtscope, query))
            using (var tcps = objsearcher.Get())
            {
                foreach (ManagementObject tcp in tcps)
                {
                    try
                    {
                        var t = new TCPIPPrinterPort()
                        {
                            Name = tcp["Name"]?.ToString(),
                            HostAddress = tcp["HostAddress"]?.ToString(),
                            PortNumber = tcp["PortNumber"]?.ToString()
                        };

                        tcpIPProtList.Add(t);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return tcpIPProtList;
        }
        #endregion

        #region + private static List<PrinterInfo> GetPrinterList()
        private static List<PrinterInfo> GetPrinterList()
        {
            var printerList = new List<PrinterInfo>();

            ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
            var query = new ObjectQuery("Select * from Win32_Printer");

            using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
            using (var printers = objsearcher.Get())
            {
                foreach (ManagementObject printer in printers)
                {
                    try
                    {
                        var p = new PrinterInfo()
                        {
                            Name = printer["Name"]?.ToString(),
                            Local = (bool)printer["Local"],
                            Network = (bool)printer["Network"],
                            PortName = printer["PortName"]?.ToString(),
                            ServerName = printer["ServerName"]?.ToString()
                        };

                        printerList.Add(p);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return printerList;
        }
        #endregion

        #region + private static List<PrinterInfo> GetTcpIPPrinterList()
        private static List<PrinterInfo> GetTcpIPPrinterList()
        {
            var tcpPrinters = new List<PrinterInfo>();

            var printers = GetPrinterList();
            if (printers.Count <= 0)
            {
                return tcpPrinters;
            }

            var tcpIPPorts = GetTcpIPPrinterPortList();
            if (tcpIPPorts.Count <= 0)
            {
                return tcpPrinters;
            }

            foreach (var tcp in tcpIPPorts)
            {
                var printer = printers.Find(p => p.PortName == tcp.Name);
                if (printer != null)
                {
                    printer.TCPIPPort = tcp;
                    tcpPrinters.Add(printer);
                }
            }

            return tcpPrinters;
        }
        #endregion

        #region + private static void DeletePrinterByName(string name)
        private static object _Locker_DeletePrinter = new object();

        private static void DeletePrinterByName(string name)
        {
            lock (_Locker_DeletePrinter)
            {
                ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
                var query = new ObjectQuery($"Select * from Win32_Printer Where Name='{name}'");

                using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var printers = objsearcher.Get())
                {
                    foreach (ManagementObject printer in printers)
                    {
                        printer.InvokeMethod("CancelAllJobs", null);
                        printer.Delete();
                    }
                }
            }
        }
        #endregion

        #region + public static void DeleteOldTcpIPPrinters()
        public static void DeleteOldTcpIPPrinters()
        {
            try
            {
                var tcpPrinters = GetTcpIPPrinterList();
                if (tcpPrinters.Count <= 0)
                {
                    return;
                }

                var com = new PerComputer();
                PerComputerHelp.SetNetworkInfo(com);

                foreach (var printer in tcpPrinters)
                {
                    try
                    {
                        var printerSubnet = PerComputerHelp.GetNetworkAddress(IPAddress.Parse(printer.GetIP()),
                                                                                IPAddress.Parse(com.IPv4Mask))
                                                                                .ToString();

                        if (com.NetwordAddress != printerSubnet)
                        {
                            DeletePrinterByName(printer.Name);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
