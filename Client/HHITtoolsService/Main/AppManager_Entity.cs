using AgentLib;
using System.Collections.Generic;

namespace HHITtoolsService
{
    public class AppManager_Entity
    {
        public static PipeServer_Service PipeServer_Service { get; set; }

        public static PrintJobNotify PrintJobNotify { get; set; }

        public static AgentTimer AppTimer { get; set; }

        public static AppProcessInfo HHITtoolsUSBApp { get; set; }

        public static List<AppProcessInfo> HHITtoolsTrayList => new List<AppProcessInfo>();

    }
}
