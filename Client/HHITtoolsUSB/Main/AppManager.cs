using AgentLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib.AppService;

namespace HHITtoolsUSB
{
    public class AppManager
    {

        #region Start()
        public static void Start()
        {
            // NamedPipeClient_USB
            AppService.NamedPipeClient = new NamedPipeClient_USB();
            AppService.NamedPipeClient.Start();

            // USBAppTimer
            AppService.USBAppTimer = new USBAppTimer();
            AppService.USBAppTimer.Start();

            // filter all usb
            try
            {
                if (AgentRegistry.UsbFilterEnabled)
                {
                    UsbHelp.UpdateUSBWhiltelist_And_FilterAllUSB();
                }
            }
            catch (Exception) { }
        }
        #endregion


        #region Stop()
        public static void Stop()
        {
            try
            {
                AppService.NamedPipeClient.Stop();
            }
            catch (Exception)
            {
            }

            try
            {
                AppService.USBAppTimer.Stop();
            }
            catch (Exception)
            {
            }
        }
        #endregion
      
    }
}
