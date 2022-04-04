using AgentLib.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;
using System.Diagnostics;

namespace HHITtoolsService
{
    public class HHITtoolsUSBService : IAppService
    {
        public Process AppProcess { get; private set; }

        public string AppFullPath { get; private set; }

        public void Start()
        {
            try
            {
                try
                {
                    AppFullPath = AgentRegistry.HHITtoolsUSBApp;
                }
                catch (Exception ex)
                {
                    throw new Exception("AgentRegistry.HHITtoolsUSBApp : " + ex.Message);
                }

                try
                {
                    AppProcess = AppProcessHelp.StartupApp(AppFullPath);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error("HHITtoolsUSBService.Start(): " + ex.GetBaseException().Message);
            }
        }

        public void Stop()
        {
            AppProcessHelp.CloseOrKillProcess(AppProcess);
        }
    }
}
