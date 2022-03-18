using System.Collections.Generic;

namespace AgentLib
{
    public class PipeMsg
    {
        public PipeMsgType PipeMsgType { get; set; }

        public UsbDisk UsbDisk { get; set; }

        public string Message { get; set; }

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
            PipeMsgType = PipeMsgType.UsbDiskNoRegister;
            UsbDisk = usbDisk;
        }

    }
}
