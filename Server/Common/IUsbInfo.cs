namespace USBCommon
{
    public interface IUsbInfo
    {
        int Vid { get; set; }

        string Vid_Hex { get; }

        int Pid { get; set; }

        string Pid_Hex { get; }

        string SerialNumber { get; set; }

        string Manufacturer { get; set; }

        string Product { get; set; }

        string DeviceDescription { get; set; }

        string UsbIdentity { get; }
    }
}
