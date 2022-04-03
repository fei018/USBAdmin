using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;
using AgentLib.AppService;

namespace HHITtoolsService
{
    public class ServiceTimer : IAppService
    {
        private AgentTimer _agentTimer;

        public AppServiceType ServiceType => AppServiceType.AgentTimer;

        public ServiceTimer()
        {
            _agentTimer = new AgentTimer();
        }

        public void Start()
        {
            _agentTimer.ElapsedAction += ElapsedAction;

            _agentTimer.Start();
        }

        public void Stop()
        {
            _agentTimer.ElapsedAction -= ElapsedAction;
            _agentTimer.Stop();
        }

        private void ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    new AgentHttpHelp().PostPerComputer_Http();
                }
                catch (Exception ex)
                {
                    AgentLogger.Error("ServiceTimer.ElapsedAction(): " + ex.Message);
                }

                try
                {
                    new AgentHttpHelp().GetAgentSetting_Http();

                    if (AgentRegistry.UsbFilterEnabled)
                    {
                        if (!AppManager.AppServicesList.Any(app=>app.ServiceType == AppServiceType.HHITtoolsUSB))
                        {
                            IAppService appUsb = new HHITtoolsUSBService();
                            appUsb.Start();
                            AppManager.AppServicesList.Add(appUsb);
                        }
                    }

                    if (AgentRegistry.PrintJobLogEnabled)
                    {
                        if (!AppManager.AppServicesList.Any(app => app.ServiceType == AppServiceType.PrintJobLog))
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error("ServiceTimer.ElapsedAction(): " + ex.Message);
                }
            });
        }     
    }
}
