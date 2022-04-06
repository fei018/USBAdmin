using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsTray
{
    public static class AppService
    {
        public static NamedPipeClient_Tray NamedPipeClient { get; set; }

        public static TrayIcon TrayIcon { get; set; }
    }
}
