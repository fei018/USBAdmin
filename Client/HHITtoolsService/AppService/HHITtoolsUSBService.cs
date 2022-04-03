using AgentLib.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;

namespace HHITtoolsService
{
    public class HHITtoolsUSBService : IAppService
    {
        private AppProcessInfo _appProcess;

        public AppServiceType ServiceType => AppServiceType.HHITtoolsUSB;

        public void Start()
        {
            string appPath;

            try
            {
                try
                {
                    appPath = AgentRegistry.HHITtoolsUSBApp;
                }
                catch (Exception ex)
                {
                    throw new Exception("AgentRegistry.HHITtoolsUSBApp : " + ex.Message);
                }

                try
                {
                    AppProcessInfo.StartupApp(appPath);
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
            _appProcess.CloseOrKillProcess();
        }
    }
}
