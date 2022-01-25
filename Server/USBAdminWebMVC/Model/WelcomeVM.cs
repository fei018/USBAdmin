using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USBAdminWebMVC
{
    public class WelcomeVM
    {
        public int UsbRequestUnderReviewCount { get; set; }

        public int UsbRequestApproveCount { get; set; }

        public int UsbRequestRejectCount { get; set; }

        public int ComputerCount { get; set; }
    }
}
