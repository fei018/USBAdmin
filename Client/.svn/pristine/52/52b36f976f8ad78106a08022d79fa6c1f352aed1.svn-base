using System;

namespace USBNotifyLib
{
    public class PipeEventArgs : EventArgs
    {
        public UsbDisk UsbDiskInfo { get; set; }

        public string Msg { get; set; }

        public PipeEventArgs()
        {
        }

        public PipeEventArgs(UsbDisk usbDisk)
        {
            UsbDiskInfo = usbDisk;
        }

        public PipeEventArgs(string msg)
        {
            Msg = msg;
        }
    }
}
