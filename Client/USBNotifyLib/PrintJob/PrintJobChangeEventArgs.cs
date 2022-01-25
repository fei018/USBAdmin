using System;
using System.Printing;

namespace USBNotifyLib.PrintMon
{
    public class PrintJobChangeEventArgs : EventArgs
    {
        #region private variables
        private int _jobID = 0;
        private string _jobName = "";
        private JOBSTATUS _jobStatus = new JOBSTATUS();
        private PrintSystemJobInfo _jobInfo = null;
        #endregion

        public int JobID { get { return _jobID; } }
        public string JobName { get { return _jobName; } }
        public JOBSTATUS JobStatus { get { return _jobStatus; } }
        public PrintSystemJobInfo JobInfo { get { return _jobInfo; } }
        public PrintJobChangeEventArgs(int intJobID, string strJobName, JOBSTATUS jStatus, PrintSystemJobInfo objJobInfo)
            : base()
        {
            _jobID = intJobID;
            _jobName = strJobName;
            _jobStatus = jStatus;
            _jobInfo = objJobInfo;
        }
    }
}
