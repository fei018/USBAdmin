using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsUSB
{
    public static class AppManager_Entity
    {
        public static HHITtoolsUSBForm HHITtoolsUSBForm { get; set; }

        public static PipeClient_USB PipeClient_USB { get; set; }

        public static void InitialEntity()
        {
            HHITtoolsUSBForm = new HHITtoolsUSBForm();
            PipeClient_USB = new PipeClient_USB();
        }
    }
}
