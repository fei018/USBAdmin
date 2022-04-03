using System;
using System.Timers;

namespace AgentLib
{
    /// <summary>
    /// Interval : ( Min : 1 minute , Max : 24 housrs )
    /// </summary>
    public class AgentTimer
    {
        private Timer _timer;

        public event EventHandler<ElapsedEventArgs> ElapsedAction;

        #region Start()
        public void Start()
        {
            try
            {
                SetTimer();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + public void Stop()
        public void Stop()
        {
            try
            {
                if (_timer != null)
                {
                    _timer.Elapsed -= _timer_Elapsed;
                    _timer.Stop();
                    _timer.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region + private void SetTimer()
        private void SetTimer()
        {
            try
            {
                _timer = new Timer();
                _timer.Interval = GetInterval();
                _timer.AutoReset = true;
                _timer.Elapsed += _timer_Elapsed;

                _timer.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Interval = GetInterval();

            try
            {
                ElapsedAction?.Invoke(sender, e);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + private double GetInterval()
        private double GetInterval()
        {
            int minutes = 10; // default minutes

            try
            {
                minutes = AgentRegistry.AgentTimerMinute;
            }
            catch (Exception)
            {
                return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
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
