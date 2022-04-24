using AgentLib;
using AgentLib.AppService;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace HHITtoolsUSB
{
    public class USBAppTimer : IAppService
    {
        private Timer _timer;

        public void Start()
        {
            try
            {
                _timer = new Timer();
                _timer.AutoReset = true;
                _timer.Interval = GetInterval();
                _timer.Elapsed += ElapsedAction;

                _timer.Start();
            }
            catch (Exception)
            {
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                try
                {
                    _timer.Elapsed -= ElapsedAction;
                    _timer.Stop();
                }
                catch (Exception)
                {
                }
            }
        }

        private void ElapsedAction(object sender, ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    new AgentHttpHelp().UpdateUSBWhitelist();
                    new UsbFilter().Filter_Scan_All_USBDisk();

                    _timerRestart();
                }
                catch (Exception ex)
                {
                    AgentLogger.Error("USBAppTimer.ElapsedAction(): " + ex.Message);
                }
            });
        }

        private void _timerRestart()
        {
            _timer.Enabled = false;
            _timer.Interval = GetInterval();
            _timer.Enabled = true;
        }

        #region + private double GetInterval()
        private double GetInterval()
        {
            int minutes;

            try
            {
                minutes = AgentRegistry.AgentTimerMinute;
            }
            catch (Exception)
            {
                return TimeSpan.FromMinutes(10).TotalMilliseconds;
            }

            // minimum 1 minutes
            if (minutes < 1)
            {
                minutes = 1;
            }

            // maximum 24 hours
            if (minutes > 1440)
            {
                minutes = 1440;
            }

            return TimeSpan.FromMinutes(minutes).TotalMilliseconds; ;
        }
        #endregion
    }
}
