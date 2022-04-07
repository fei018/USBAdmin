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
                    new AgentHttpHelp().PostComputerInfo_Http();
                }
                catch (Exception ex)
                {
                    AgentLogger.Error("ServiceTimer.ElapsedAction(): " + ex.Message);
                }

                try
                {
                    new AgentHttpHelp().GetAgentRule_Http();

                    if (AgentRegistry.UsbFilterEnabled)
                    {
                        if (AppService.HHITtoolsUSB == null)
                        {
                            AppService.HHITtoolsUSB = new HHITtoolsUSBService();
                            AppService.HHITtoolsUSB.Start();
                        }
                        else if(AppService.HHITtoolsUSB.AppProcess == null)
                        {
                            AppService.HHITtoolsUSB = new HHITtoolsUSBService();
                            AppService.HHITtoolsUSB.Start();
                        }
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error("ServiceTimer.ElapsedAction(): " + ex.Message);
                }

                try
                {
                    if (AgentRegistry.PrintJobLogEnabled)
                    {
                        if (AppService.PrintJobLogService == null)
                        {
                            AppService.PrintJobLogService = new PrintJobLogService();
                            AppService.PrintJobLogService.Start();
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
