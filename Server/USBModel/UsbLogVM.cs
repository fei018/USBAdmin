namespace USBModel
{
    public class UsbLogVM : Tbl_UsbLog
    {
        public string ComputerName { get; set; }


        public UsbLogVM(Tbl_UsbLog usbHistory, Tbl_ComputerInfo com = null)
        {
            ComputerName = com?.HostName;
            ComputerIdentity = com?.ComputerIdentity;

            Vid = usbHistory.Vid;
            Pid = usbHistory.Pid;
            SerialNumber = usbHistory.SerialNumber;
            DeviceDescription = usbHistory.DeviceDescription;
            Product = usbHistory.Product;
            Manufacturer = usbHistory.Manufacturer;
            PluginTime = usbHistory.PluginTime;            
        }
    }
}
