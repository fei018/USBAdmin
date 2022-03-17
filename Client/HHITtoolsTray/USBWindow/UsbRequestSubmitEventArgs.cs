using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsTray.USBWindow
{
    public class UsbRequestSubmitEventArgs : EventArgs
    {
        public string UserEmailAddress { get; set; }

        public string RequestReason { get; set; }
    }
}
