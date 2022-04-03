using AgentLib.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;

namespace HHITtoolsService
{
    public class HHITtoolsTrayService : IAppService
    {
        private AppProcessInfo _appProcess;

        public AppServiceType ServiceType => AppServiceType.HHITtoolsTray;

        public void Start()
        {
            string appPath;

            try
            {
                try
                {
                    appPath = AgentRegistry.HHITtoolsTrayApp;
                }
                catch (Exception ex)
                {
                    throw new Exception("AgentRegistry.HHITtoolsTrayApp : " + ex.Message);
                }

                try
                {
                    AppProcessInfo.StartupAppAsLogonUser(appPath);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error("HHITtoolsTrayService.Start(): " + ex.GetBaseException().Message);
            }
        }

        public void Stop()
        {
            _appProcess.CloseOrKillProcess();
        }
    }
}
