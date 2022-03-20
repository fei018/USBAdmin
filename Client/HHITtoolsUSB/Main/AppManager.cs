using AgentLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsUSB
{
    public class AppManager
    {
        #region Startup()
        public static void Startup()
        {
            AppManager_Entity.PipeClient_USB = new PipeClient_USB();

            AppManager_Entity.PipeClient_USB.Start();
        }
        #endregion

        #region Close()
        public static void Close()
        {
            AppManager_Entity.PipeClient_USB?.Stop();
        }
        #endregion
      
    }
}
