using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsMeshAgentNotify
{
    public static class AppManger
    {
        public static void Start()
        {
            //--- NamedPipeClient_MeshAgentNotify
            AppService.NamedPipeClient_MeshAgentNotify = new NamedPipeClient_MeshAgentNotify();
            AppService.NamedPipeClient_MeshAgentNotify.Start();

            //--- MeshAgentServiceHelp, Start at MainWindow
            AppService.MeshAgentServiceHelp = new MeshAgentServiceHelp();
        }

        public static void Stop()
        {
            //--- MeshAgentServiceHelp
            AppService.MeshAgentServiceHelp?.Stop();


            //--- NamedPipeClient_MeshAgentNotify
            AppService.NamedPipeClient_MeshAgentNotify?.Stop();
        }
    }
}
