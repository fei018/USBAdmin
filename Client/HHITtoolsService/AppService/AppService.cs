using AgentLib.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;

namespace HHITtoolsService
{
    public static class AppService
    {
        public static List<HHITtoolsTrayService> HHITtoolsTrayList => new List<HHITtoolsTrayService>();

        public static NamedPipeServer_Service NamedPipeServer { get; set; }

        public static HHITtoolsUSBService HHITtoolsUSB{ get; set; }

        public static PrintJobLogService PrintJobLogService { get; set; }

        public static ServiceTimer ServiceTimer { get; set; }
    }
}
