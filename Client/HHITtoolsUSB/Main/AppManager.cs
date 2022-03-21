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
            AppManager_Entity.Initial();

            AppManager_Entity.PipeClient_USB?.Start();

            AppManager_Entity.AppTimer.ElapsedAction += AgentTimer_ElapsedAction;
            AppManager_Entity.AppTimer.Start();

            UsbHelp.UpdateUSBWhiltelist_And_FilterAllUSB();
        }
        #endregion

        #region + private static void AgentTimer_ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        private static void AgentTimer_ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    new AgentHttpHelp().UpdateUSBWhitelist_Http();
                    new UsbFilter().Filter_Scan_All_USB_Disk();
                });
            }
            catch (Exception)
            {
            }
        }
        #endregion


        #region Close()
        public static void Close()
        {
            AppManager_Entity.PipeClient_USB?.Stop();

            AppManager_Entity.AppTimer.Stop();
        }
        #endregion
      
    }
}
