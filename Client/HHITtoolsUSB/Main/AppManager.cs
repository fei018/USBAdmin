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
            UsbHelp.UpdateUSBWhiltelist_And_FilterAllUSB();
        }
        #endregion


        #region Stop()
        public static void Stop()
        {
            try
            {

            }
            catch (Exception)
            {
            }
        }
        #endregion
      
    }
}
