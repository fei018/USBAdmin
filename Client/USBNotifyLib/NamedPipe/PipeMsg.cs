using System.Collections.Generic;
using USBNotifyLib;

namespace USBNotifyLib
{
    public class PipeMsg
    {
        public PipeMsgType PipeMsgType { get; set; }

        public UsbDisk UsbDisk { get; set; }

        public string Message { get; set; }

        public string PrintTemplateFile { get; set; }

        public List<IPPrinterInfo> AddPrinterDriverList { get; set; }

        public SitePrinterToAddList SitePrinterToAddList { get; set; }

        public PipeMsg() { }

        public PipeMsg(PipeMsgType msgType)
        {
            PipeMsgType = msgType;
        }

        public PipeMsg(PipeMsgType msgType, string message)
        {
            PipeMsgType = msgType;
            Message = message;
        }

        public PipeMsg(UsbDisk usbDisk)
        {
            PipeMsgType = PipeMsgType.UsbDiskNotInWhitelist;
            UsbDisk = usbDisk;
        }
    }
}
