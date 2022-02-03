using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Printing;
using System.Text;

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
        private static List<LocalPrinterInfo> GetPrinterList()
        {
            var printerList = new List<LocalPrinterInfo>();

            ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
            var query = new ObjectQuery("Select * from Win32_Printer");

            using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
            using (var printers = objsearcher.Get())
            {
                foreach (ManagementObject printer in printers)
                {
                    try
                    {
                        var p = new LocalPrinterInfo()
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
        private static List<LocalPrinterInfo> GetTcpIPPrinterList()
        {
            var tcpPrinters = new List<LocalPrinterInfo>();

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

        // delete printer method

        #region + public static void DeletePrinterByName(string name)
        private static object _Locker_DeletePrinter = new object();

        public static void DeletePrinter_ByName(string name)
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
                        printer.Dispose();
                    }
                }
            }
        }
        #endregion

        #region + public static void DeleteTcpIPPort_ByName(string name)
        public static void DeleteTcpIPPort_ByName(string name)
        {
            lock (_Locker_DeletePrinter)
            {
                ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
                var query = new ObjectQuery($"Select * from Win32_TCPIPPrinterPort Where Name='{name}'");

                using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var tcps = objsearcher.Get())
                {
                    foreach (ManagementObject tcp in tcps)
                    {
                        tcp.Delete();
                        tcp.Dispose();
                    }
                }
            }
        }
        #endregion

        #region + public static void DeleteOldIPPrinters_OtherSubnet()
        public static void DeleteOldIPPrinters_OtherSubnet()
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
                            DeletePrinter_ByName(printer.Name);

                            DeleteTcpIPPort_ByName(printer.TCPIPPort.Name);
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


        // add printer method

        #region + public static void AddNewPrinter(IPPrinterInfo printer)
        public static void AddNewPrinter(IPPrinterInfo printer)
        {
            try
            {
                if (!PrinterDriverExist(printer.DriverName))
                {
                    throw new Exception("Printer driver not exist: " + printer.DriverName);
                }

                if (!AddOrUpdatePrinterIPPort(printer.PortIPAddr, out string error))
                {
                    throw new Exception(error);
                }

                if (!AddPrinter_Printing(printer.PrinterName, printer.DriverName, printer.PortIPAddr, out string error2))
                {
                    throw new Exception(error2);
                }

                //SetPrinterConfig_WMI(printer.PrinterName);
                SetPrinterConfig_Printing(printer.PrinterName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static bool PrinterExist(string printerName)
        private static bool PrinterExist(string printerName)
        {
            try
            {
                ManagementScope mgmtscope = new ManagementScope(@"\root\StandardCimv2");
                var query = new ObjectQuery($"Select * from MSFT_Printer Where Name = '{printerName}'");

                using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var printers = objsearcher.Get())
                {
                    if (printers.Count >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public static bool PrinterDriverExist(string driverName)
        public static bool PrinterDriverExist(string driverName)
        {
            try
            {
                ManagementScope mgmtscope = new ManagementScope(@"\ROOT\StandardCimv2");
                var query = new ObjectQuery($"Select * from MSFT_PrinterDriver Where Name = '{driverName}'");

                using (ManagementObjectSearcher objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (ManagementObjectCollection drivers = objsearcher.Get())
                {
                    if (drivers.Count >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static bool AddPrinter_Printing(string printerName, string driverName, string portName, out string error)
        private static bool AddPrinter_Printing(string printerName, string driverName, string portName, out string error)
        {
            error = null;
            try
            {
                using (var printServer = new PrintServer())
                using (var printQ = printServer.InstallPrintQueue(printerName,
                                                   driverName,
                                                   new string[] { portName },
                                                   "winprint",
                                                   PrintQueueAttributes.ScheduleCompletedJobsFirst))
                {
                    if (printQ != null)
                    {
                        return true;
                    }
                    else
                    {
                        error = "Add printer fail: " + printerName;
                        return false;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static bool AddPrinter_WMI(string printerName, string driverName, string portName, out string error)
        private static bool AddPrinter_WMI(string printerName, string driverName, string portName, out string error)
        {
            error = null;
            try
            {
                using (ManagementClass mc = new ManagementClass(@"\root\StandardCimv2:MSFT_Printer"))
                using (ManagementObject printerObj = mc.CreateInstance())
                {
                    string methodName = "AddByExistingPort";

                    var inParam = mc.GetMethodParameters(methodName);
                    inParam["Name"] = printerName;
                    inParam["DriverName"] = driverName;
                    inParam["PortName"] = portName;

                    var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromMinutes(5));
                    mc.InvokeMethod(methodName, inParam, invokeOption);
                    mc.Dispose();
                }

                // check add succeed

                ManagementScope mgmtscope = new ManagementScope(@"\root\StandardCimv2");
                var query = new ObjectQuery($"Select * from MSFT_Printer Where Name = '{printerName}'");

                using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var printers = objsearcher.Get())
                {
                    if (printers.Count >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("Add Printer Fail: " + printerName);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.GetBaseException().Message;
                return false;
            }
        }
        #endregion

        #region + private static bool AddOrUpdatePrinterIPPort(string ipAddress, out string error)
        private static bool AddOrUpdatePrinterIPPort(string ipAddress, out string error)
        {
            error = null;
            try
            {
                using (ManagementClass portClass = new ManagementClass(@"\root\cimv2:Win32_TCPIPPrinterPort"))
                using (ManagementObject portObject = portClass.CreateInstance())
                {
                    portObject["Name"] = ipAddress;
                    portObject["HostAddress"] = ipAddress;
                    portObject["PortNumber"] = 9100;
                    portObject["Protocol"] = 1;
                    portObject["SNMPCommunity"] = "public";
                    portObject["SNMPEnabled"] = false;
                    portObject["SNMPDevIndex"] = 1;

                    PutOptions options = new PutOptions(null, TimeSpan.FromSeconds(30), false, PutType.UpdateOrCreate);
                    //options.Type = PutType.UpdateOrCreate;
                    //put a newly created object to WMI objects set             
                    portObject.Put(options);
                }

                // check whether add succeed
                ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
                var query = new ObjectQuery($"Select * from Win32_TCPIPPrinterPort Where Name = '{ipAddress}'");

                using (ManagementObjectSearcher objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var tcps = objsearcher.Get())
                {
                    if (tcps.Count >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        error = "Create Or Update Printer TCPIP Port Fail.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.GetBaseException().Message;
                return false;
            }
        }
        #endregion

        #region + private static bool AddPrinterDriver_WMI(string driverName, string infPath)
        public static bool AddPrinterDriver_WMI(string driverName, string infPath)
        {
            try
            {
                using (ManagementClass mc = new ManagementClass(@"\root\cimv2:Win32_PrinterDriver"))
                using (var dirver = mc.CreateInstance())
                {
                    dirver["Name"] = driverName;
                    dirver["InfName"] = infPath;
                    //dirver["FilePath"] = "";

                    var inParam = mc.GetMethodParameters("AddPrinterDriver");
                    inParam["DriverInfo"] = dirver;

                    var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromMinutes(5));
                    mc.InvokeMethod("AddPrinterDriver", inParam, invokeOption);
                }

                // check whether driver add succeed

                if (PrinterDriverExist(driverName))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Add printer driver fail: " + driverName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region public static bool SetPrinterConfig_Printing(string printerName)
        public static bool SetPrinterConfig_Printing(string printerName)
        {
            try
            {
                using (var printServer = new PrintServer())
                using (var printQ = printServer.GetPrintQueue(printerName))
                {
                    printQ.UserPrintTicket.Duplexing = Duplexing.OneSided;
                    printQ.UserPrintTicket.OutputColor = OutputColor.Monochrome;
                    printQ.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void SetPrinterConfig_WMI(string printerName)
        private static void SetPrinterConfig_WMI(string printerName)
        {
            /**
             * DuplexingMode:
             *      OneSided = 0
             *      TwoSidedLongEdge = 1
             *      TwoSidedShortEdge = 2
             * 
             * Color:
             *      black = false
             *      color = true
            **/

            try
            {
                using (ManagementClass mc = new ManagementClass(@"\root\StandardCimv2:MSFT_PrinterConfiguration"))
                using (var inParams = mc.GetMethodParameters("SetByPrinterName"))
                {
                    inParams["PrinterName"] = printerName;
                    inParams["Color"] = false;
                    inParams["DuplexingMode"] = 0;

                    var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromSeconds(30));
                    mc.InvokeMethod("SetByPrinterName", inParams, invokeOption);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


    }
}
