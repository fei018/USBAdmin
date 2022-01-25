using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBModel
{
    public class UsbRequestVM : Tbl_UsbRequest
    {
        public string ComputerName { get; set; }

        public string IP { get; set; }



        public UsbRequestVM(Tbl_UsbRequest usbRequest, Tbl_PerComputer com = null)
        {
            ComputerName = com?.HostName;
            RequestComputerIdentity = com?.ComputerIdentity;
            IP = com?.IPAddress;

            Id = usbRequest.Id;
            Vid = usbRequest.Vid;
            Pid = usbRequest.Pid;
            SerialNumber = usbRequest.SerialNumber;
            DeviceDescription = usbRequest.DeviceDescription;
            Product = usbRequest.Product;
            Manufacturer = usbRequest.Manufacturer;
            RequestTime = usbRequest.RequestTime;
            RequestStateChangeTime = usbRequest.RequestStateChangeTime;
            RequestState = usbRequest.RequestState;
            RequestReason = usbRequest.RequestReason;
            RequestUserEmail = usbRequest.RequestUserEmail;
        }
    }
}
