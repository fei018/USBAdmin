using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Printing;
using USBCommon;

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
                    finally
                    {
                        printer.Dispose();
                    }
                }
            }

            return printerList;
        }
        #endregion

        #region + public static List<PrinterInfo> GetIPPrinterList()
        public static List<LocalPrinterInfo> GetIPPrinterList()
        {
            var ipPrinters = new List<LocalPrinterInfo>();

            var printers = GetPrinterList();
            if (printers.Count <= 0)
            {
                return ipPrinters;
            }

            var tcpIPPorts = GetTcpIPPrinterPortList();
            if (tcpIPPorts.Count <= 0)
            {
                return ipPrinters;
            }

            foreach (var port in tcpIPPorts)
            {
                var printer = printers.Find(p => p.PortName == port.Name);
                if (printer != null)
                {
                    printer.TCPIPPort = port;
                    ipPrinters.Add(printer);
                }
            }

            return ipPrinters;
        }
        #endregion

        //// delete printer method

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
                var tcpPrinters = GetIPPrinterList();
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
                        var printerSubnet = UtilityTools.GetNetworkAddress(IPAddress.Parse(printer.GetIP()),
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


        //// add printer method

        // public

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

                SetPrinterConfig_WMI(printer.PrinterName);
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
                    int count = drivers.Count;
                    drivers.Dispose();
                    objsearcher.Dispose();

                    if (count >= 1)
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


        // private

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
                        try
                        {
                            printQ.DefaultPrintTicket.OutputColor = OutputColor.Monochrome;
                            printQ.DefaultPrintTicket.Duplexing = Duplexing.OneSided;
                            printQ.Commit();
                        }
                        catch (Exception) { }

                        try
                        {
                            printQ.UserPrintTicket.OutputColor = OutputColor.Monochrome;
                            printQ.UserPrintTicket.Duplexing = Duplexing.OneSided;
                            printQ.Commit();
                        }
                        catch (Exception) { }

                        printQ.Dispose();
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

                    //var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromMinutes(5));
                    mc.InvokeMethod(methodName, inParam, null);
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

        #region + private static bool PrinterIPPortExist(string ipAddress)
        private static bool PrinterIPPortExist(string ipAddress)
        {
            try
            {
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

                if (PrinterIPPortExist(ipAddress))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Create Or Update Printer TCPIP Port Fail: " + ipAddress);
                }
            }
            catch (Exception ex)
            {
                error = ex.GetBaseException().Message;
                return false;
            }
        }
        #endregion

        #region + private static bool InstallPrinterDriver_WMI(string driverName, string infPath)
        public static bool InstallPrinterDriver_WMI(string driverName, string infPath)
        {
            try
            {
                var mScope = CreateManagementScope_Cimv2();

                using (ManagementClass mc = new ManagementClass(mScope, new ManagementPath("Win32_PrinterDriver"), new ObjectGetOptions()))
                using (var driverInfo = mc.CreateInstance())
                {
                    driverInfo.SetPropertyValue("Name", driverName);
                    driverInfo.SetPropertyValue("InfName", infPath);
                    //dirver["FilePath"] = infDir;

                    using (var methodParam = mc.GetMethodParameters("AddPrinterDriver"))
                    {
                        methodParam["DriverInfo"] = driverInfo;

                        //var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromMinutes(5));
                        var result = mc.InvokeMethod("AddPrinterDriver", methodParam, null);
                        var code = (int)result.Properties["ReturnValue"].Value;

                        if (code != 0)
                        {
                            throw new Exception("Install printer driver fail, return code: " + code);
                        }
                    }
                }

                // check whether driver add succeed

                if (PrinterDriverExist(driverName))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Install printer driver fail: " + driverName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static bool SetPrinterConfig_Printing(string printerName)
        private static void SetPrinterConfig_Printing(string printerName)
        {
            try
            {
                using (var pServer = new PrintServer())
                using (var printQ = pServer.GetPrintQueue(printerName))
                {
                    try
                    {
                        printQ.DefaultPrintTicket.Duplexing = Duplexing.OneSided;
                        printQ.DefaultPrintTicket.OutputColor = OutputColor.Grayscale;
                        printQ.Commit();
                    }
                    catch (Exception) { }

                    try
                    {
                        printQ.UserPrintTicket.Duplexing = Duplexing.OneSided;
                        printQ.UserPrintTicket.OutputColor = OutputColor.Grayscale;
                        printQ.Commit();
                    }
                    catch (Exception) { }

                    printQ.Dispose();
                    pServer.Dispose();
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
                var mScope = CreateManagementScope_StandardCimv2();
                using (ManagementClass mc = new ManagementClass(mScope, new ManagementPath("MSFT_PrinterConfiguration"), new ObjectGetOptions()))
                using (var methodParams = mc.GetMethodParameters("SetByPrinterName"))
                {
                    methodParams.SetPropertyValue("PrinterName", printerName);
                    methodParams.SetPropertyValue("Color", false);
                    methodParams.SetPropertyValue("DuplexingMode", 0);

                    //var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromSeconds(30));
                    mc.InvokeMethod("SetByPrinterName", methodParams, null);

                    methodParams.Dispose();
                    mc.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        //// CreateManagementScope

        #region + private static ManagementScope CreateManagementScope_Cimv2()
        private static ManagementScope CreateManagementScope_Cimv2()
        {
            var wmiConnectionOptions = new ConnectionOptions();
            wmiConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
            wmiConnectionOptions.Authentication = AuthenticationLevel.Default;
            wmiConnectionOptions.EnablePrivileges = true; // required to load/install the driver.
                                                          // Supposed equivalent to VBScript objWMIService.Security_.Privileges.AddAsString "SeLoadDriverPrivilege", True 

            var managementScope = new ManagementScope("\\root\\cimv2", wmiConnectionOptions);
            managementScope.Connect();
            return managementScope;
        }
        #endregion

        #region + private static ManagementScope CreateManagementScope_StandardCimv2()
        private static ManagementScope CreateManagementScope_StandardCimv2()
        {
            var wmiConnectionOptions = new ConnectionOptions();
            wmiConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
            wmiConnectionOptions.Authentication = AuthenticationLevel.Default;
            wmiConnectionOptions.EnablePrivileges = true; // required to load/install the driver.
                                                          // Supposed equivalent to VBScript objWMIService.Security_.Privileges.AddAsString "SeLoadDriverPrivilege", True 

            var managementScope = new ManagementScope("\\root\\StandardCimv2", wmiConnectionOptions);
            managementScope.Connect();
            return managementScope;
        }
        #endregion
    }
}
