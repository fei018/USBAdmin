using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgentLib.Win32API;
using AgentLib;
using AgentLib.PrintJob;
using System.Printing;
using System.Management;
using System.Printing.IndexedProperties;
using System.Collections;

namespace USBTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start...");

                //var p = new IPPrinterInfo();
                //p.PrinterName = "test printer111";
                //p.DriverName   = "Kyocera TASKalfa 3554ci KX";
                //p.DriverInfPath = @"C:\Users\User\Downloads\64bit\OEMSETUP.INF";
                //p.PortIPAddr = "10.20.4.5";
                //PrinterHelp.InstallPrinterDriver_WMI(p.DriverName, p.DriverInfPath);
                //PrinterHelp.AddNewPrinter(p);

                //PrintJobNotify.Entity = new PrintJobNotify();
                //PrintJobNotify.Entity.Start();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.WriteLine("!!!!!!!!!!!!!!!!!!$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            }

            Console.WriteLine("##############################################");
            Console.ReadLine();

            //PrintJobNotify.Entity.Stop();
        }

        #region UsbFormProcess
        private Process _usbFormProcess;
        private bool IsBootUsbForm = true;

        void OnStart()
        {
            IsBootUsbForm = true;
            StartUsbFormProcess();
        }

        void OnStop()
        {
            IsBootUsbForm = false;
            CloseUsbFormProcess();
        }

        private void StartUsbFormProcess()
        {
            try
            {
                CloseUsbFormProcess();

                ProcessStartInfo startInfo = new ProcessStartInfo();
                _usbFormProcess = new Process
                {
                    EnableRaisingEvents = true,
                    StartInfo = startInfo
                };

                _usbFormProcess.Exited += usbProcess_Exited;

                _usbFormProcess.Start();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }

        private void CloseUsbFormProcess()
        {
            try
            {
                if (_usbFormProcess != null && !_usbFormProcess.HasExited)
                {
                    if (!_usbFormProcess.CloseMainWindow())
                    {
                        _usbFormProcess.Kill();
                    }
                    _usbFormProcess.Close();
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }

        private void usbProcess_Exited(object sender, EventArgs e)
        {
            if (IsBootUsbForm)
            {
                StartUsbFormProcess();
            }

            AgentLogger.Log("USBFormApp Process Exied Event.");
        }
        #endregion

        #region CreateFile test
        static void CreateFile_OnlyRead()
        {
            //string path = @"\\?\scsi#disk&ven_samsung&prod_mzvlb512hbjq-000#6&25097e69&0&000000#{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";
            string path = @"\\.\G:\";

            var handle = UFileApi.CreateFile_ReadOnly(path);
            Console.WriteLine(handle);
            Console.ReadLine();

            UFileApi.CloseHandle(handle);
        }
        #endregion



        #region print
        static void Print()
        {
            var printMon = new PrintQueueMonitor("ADMColorTest (10.13.8.61)");
            printMon.OnJobStatusChange += PrintMon_OnJobStatusChange;

            Console.ReadLine();
            printMon.Stop();
        }

        private static void PrintMon_OnJobStatusChange(object Sender, PrintJobChangeEventArgs e)
        {
            if (e.JobInfo != null && e.JobInfo.JobStatus == (System.Printing.PrintJobStatus.Printing | System.Printing.PrintJobStatus.Retained))
            {
                Console.WriteLine("JobStatus: " + e.JobInfo.JobStatus);
                Console.WriteLine("JobName: " + e.JobInfo.JobName);
                Console.WriteLine("Name: " + e.JobInfo.Name);
                Console.WriteLine("NumberOfPages: " + e.JobInfo.NumberOfPages);
                Console.WriteLine("Submitter: " + e.JobInfo.Submitter);
                Console.WriteLine("TimeJobSubmitted: " + e.JobInfo.TimeJobSubmitted.ToString("G"));
                Console.WriteLine("JobIdentifier: " + e.JobInfo.JobIdentifier);
                Console.WriteLine("IsCompleted: " + e.JobInfo.IsCompleted);
                Console.WriteLine("IsPrinted: " + e.JobInfo.IsPrinted);
                Console.WriteLine("IsPrinting: " + e.JobInfo.IsPrinting);
                Console.WriteLine("IsRetained: " + e.JobInfo.IsRetained);
            }
            Console.WriteLine("-----------------");
            Console.WriteLine();
        }
        #endregion

        #region Template()
        static void Template()
        {
            //PrintTemplateHelp.Start();
        }
        #endregion

        #region print driver
        static void PrintDriver()
        {
            ManagementClass mc = new ManagementClass(@"\\.\root\Cimv2:Win32_PrinterConfiguration");
            var objs = mc.GetInstances();
            foreach (ManagementBaseObject o in objs)
            {
                var ps = o.Properties;
                foreach (var p in ps)
                {
                    Console.WriteLine(p.Name+" = "+ p.Value);
                }
                break;
            }

            //ManagementScope mgmtscope = new ManagementScope(@"\root\StandardCimv2");
            //var query = new ObjectQuery("Select * from MSFT_Printer");

            //using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
            //using (var drivers = objsearcher.Get())
            //{
            //    foreach (ManagementObject driver in drivers)
            //    {
            //        var ev = driver.ge
            //    }
            //}

            //ManagementClass mc = new ManagementClass(@"\\.\root\StandardCimv2:MSFT_PrinterConfiguration");
            //var inParams = mc.GetMethodParameters("SetByPrinterName");
            //inParams["Color"] = false;
            //inParams["PrinterName"] = "Ricoh PCL6 V4";
            //inParams["DuplexingMode"] = 3;
            //mc.InvokeMethod("SetByPrinterName", inParams, null);

            //using (ManagementClass mc = new ManagementClass(@"\root\cimv2:Win32_PrinterDriver"))
            //using (var dirver = mc.CreateInstance())
            //{
            //    dirver["Name"] = "RICOH MP C4504 PCL 6";
            //    dirver["InfName"] = @"C:\temp\z94580L16\disk1\oemsetup.inf";
            //    //dirver["FilePath"] = "";

            //    var inParam = mc.GetMethodParameters("AddPrinterDriver");
            //    inParam["DriverInfo"] = dirver;

            //    var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromSeconds(30));
            //    mc.InvokeMethod("AddPrinterDriver", inParam, invokeOption);
            //}


            //using (ManagementClass portClass = new ManagementClass(@"\root\cimv2:Win32_TCPIPPrinterPort"))
            //using (ManagementObject portObject = portClass.CreateInstance())
            //{
            //    portObject["Name"] = "test_10.20.11.111";
            //    portObject["HostAddress"] = "174.30.164.15";
            //    portObject["PortNumber"] = 9100;
            //    portObject["Protocol"] = 1;
            //    portObject["SNMPCommunity"] = "public";
            //    portObject["SNMPEnabled"] = true;
            //    portObject["SNMPDevIndex"] = 1;

            //    PutOptions options = new PutOptions(null, TimeSpan.FromSeconds(10), false, PutType.UpdateOrCreate);
            //    //options.Type = PutType.UpdateOrCreate;
            //    //put a newly created object to WMI objects set             
            //    portObject.Put(options);
            //}

        }
        #endregion

 
    }
}
