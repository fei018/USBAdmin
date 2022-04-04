using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsUSB
{
    public static class AppService
    {
        public static NamedPipeClient_USB NamedPipeClient { get; set; }

        public static USBAppTimer USBAppTimer { get; set; }
    }
}
