using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsMeshAgentNotify
{
    public class AppService
    {
        public static MeshAgentServiceHelp  MeshAgentServiceHelp { get; set; }

        public static NamedPipeClient_MeshAgentNotify NamedPipeClient_MeshAgentNotify { get; set; }
    }
}
