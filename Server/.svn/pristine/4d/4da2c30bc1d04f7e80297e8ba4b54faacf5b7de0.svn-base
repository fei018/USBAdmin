namespace USBModel
{
    public class PerUsbHistoryVM : Tbl_PerUsbHistory
    {
        public string ComputerName { get; set; }


        public PerUsbHistoryVM(Tbl_PerUsbHistory usbHistory, Tbl_PerComputer com = null)
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
