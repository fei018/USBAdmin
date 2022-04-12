using AgentLib.AppService;
using AgentLib.PrintJob;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgentLib
{
    public class PrintJobLogService : IAppService
    {
        private Dictionary<string, PrintQueueMonitor> _PrintJobMonitorList { get; set; }

        #region Start
        public void Start()
        {
            Stop();

            Task.Run(() =>
            {
                try
                {
                    _PrintJobMonitorList = _PrintJobMonitorList ?? new Dictionary<string, PrintQueueMonitor>();

                    var ipPrinters = PrinterHelp.GetIPPrinterList();
                    foreach (var printer in ipPrinters)
                    {
                        try
                        {
                            var printJobMonitor = new PrintQueueMonitor(printer.Name);

                            printJobMonitor.OnJobStatusChange += PrintJobMonitor_OnJobStatusChange;

                            // start printJob monitor
                            printJobMonitor.Start();

                            _PrintJobMonitorList.Add(printJobMonitor.PrinterName, printJobMonitor);
                        }
                        catch (Exception ex)
                        {
                            AgentLogger.Error(ex.GetBaseException().Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.GetBaseException().Message);
                }
            });
        }
        #endregion

        #region Stop
        public void Stop()
        {
            try
            {
                // stop all PrintJobMonitor
                if (_PrintJobMonitorList != null && _PrintJobMonitorList.Count > 0)
                {
                    foreach (var printQMon in _PrintJobMonitorList)
                    {
                        printQMon.Value.OnJobStatusChange -= PrintJobMonitor_OnJobStatusChange;
                        printQMon.Value.Stop();
                    }

                    _PrintJobMonitorList.Clear();
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region + private void PrintJobMonitor_OnJobStatusChange(object Sender, PrintJobChangeEventArgs e)
        private static object _Locker = new object();
        static int _JobID = -1;
        private void PrintJobMonitor_OnJobStatusChange(object Sender, PrintJobChangeEventArgs e)
        {
            var jobInfo = e.JobInfo;
            if (jobInfo == null) return;

            try
            {
                if ((jobInfo.JobStatus & PrintJobStatus.Printing) == (PrintJobStatus.Printing))
                {
                    lock (_Locker)
                    {
                        if (_JobID == jobInfo.JobIdentifier)
                        {
                            return;
                        }
                        _JobID = jobInfo.JobIdentifier;
                    }

                    var job = new PrintJobLog()
                    {
                        JobId = jobInfo.JobIdentifier,
                        ComputerName = jobInfo.HostingPrintServer.Name,
                        FileName = jobInfo.Name,
                        FilePages = jobInfo.NumberOfPages,
                        PrinterName = jobInfo.HostingPrintQueue.Name,
                        PrintingTime = jobInfo.TimeJobSubmitted.ToLocalTime(),
                        UserName = jobInfo.Submitter,
                        ComputerIdentity = ComputerInfoHelp.GetComputerIdentity()
                    };

#if DEBUG
                    //Debugger.Break();
#endif
                    Task.Run(() =>
                    {
                        if (AgentRegistry.PrintJobLogEnabled)
                        {
                            try
                            {
                                new AgentHttpHelp().PostPrintJobLog(job);
                            }
                            catch (Exception ex)
                            {
                                AgentLogger.Error(ex.Message);
                            }
                        }
                    });

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                jobInfo?.Dispose();
            }
        }
        #endregion

        #region MyRegion
        private void GetLocalPrinterName()
        {
            try
            {
                // get all IP Printer
                using (var pQServer = new LocalPrintServer())
                using (var printerList = pQServer.GetPrintQueues())
                {
                    foreach (var printer in printerList)
                    {
                        if (Regex.IsMatch(printer.Name, "(Fax)+", RegexOptions.IgnoreCase))
                        {
                            continue;
                        }

                        if (Regex.IsMatch(printer.Name, "(PDF)+", RegexOptions.IgnoreCase))
                        {
                            continue;
                        }

                        var printJobMonitor = new PrintQueueMonitor(printer.Name);
                        printer.Dispose();

                        _PrintJobMonitorList.Add(printJobMonitor.PrinterName, printJobMonitor);

                        printJobMonitor.OnJobStatusChange += PrintJobMonitor_OnJobStatusChange;

                        // start printJob monitor
                        printJobMonitor.Start();
                    }
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
