using AgentLib;
using AgentLib.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsUSB
{
    public class USBAppTimer : IAppService
    {
        private AgentTimer _agentTimer;

        public USBAppTimer()
        {
            _agentTimer = new AgentTimer();
        }

        public void Start()
        {
            _agentTimer.ElapsedAction += _agentTimer_ElapsedAction;
        }

        public void Stop()
        {
            _agentTimer.ElapsedAction -= _agentTimer_ElapsedAction;
            _agentTimer.Stop();
        }

        private void _agentTimer_ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    new AgentHttpHelp().UpdateUSBWhitelist();
                    new UsbFilter().Filter_Scan_All_USB_Disk();
                }
                catch (Exception)
                {
                }
            });
        }
    }
}
