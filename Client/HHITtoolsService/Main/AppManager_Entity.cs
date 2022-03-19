using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;

namespace HHITtoolsService
{
    public class AppManager_Entity
    {
        public static PipeServer_Service PipeServer_Service { get; set; }

        public static PrintJobNotify PrintJobNotify { get; set; }

        public static void Initial()
        {
            try
            {
                PipeServer_Service = new PipeServer_Service();

                PrintJobNotify = new PrintJobNotify();
            }
            catch (Exception)
            {
            }
        }
    }
}
