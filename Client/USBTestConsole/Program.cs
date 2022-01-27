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
using USBNotifyLib.Win32API;
using USBNotifyLib;
using MailKit.Net.Smtp;
using MimeKit;
using USBNotifyLib.PrintJob;
using System.Printing;

namespace USBTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start...");

                PrinterHelp.DeleteOldTcpIPPrinters();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            }

            Console.WriteLine("##############################################");
           Console.ReadLine();
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

        #region email
        static void Email()
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("USBAdmin",
                "e_fei_huang@hiphing.com.hk");
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("fei",
                "e_fei_huang@hiphing.com.hk");
                message.To.Add(to);

                message.Subject = "USBAdmin Test";
                BodyBuilder bodyBuilder = new BodyBuilder { TextBody = "test!!!" };
                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("mailgw.hiphing.com.hk", 25,false);
                    //client.Authenticate("e_fei", "fei");

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
               
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
            if (e.JobInfo != null && e.JobInfo.JobStatus == (System.Printing.PrintJobStatus.Printing| System.Printing.PrintJobStatus.Retained))
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
    }
}
