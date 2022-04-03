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
        public static List<IAppService> AppServiceList => new List<IAppService>();

        #region MyRegion

        #endregion

        #region Startup()
        public static void Startup()
        {
            // NamedPipeClient_USB
            IAppService namedPipeUSB = new NamedPipeClient_USB();
            namedPipeUSB.Start();
            AppServiceList.Add(namedPipeUSB);

            // USBAppTimer
            IAppService apptimer = new USBAppTimer();
            apptimer.Start();
            AppServiceList.Add(apptimer);

            // filter all usb
            UsbHelp.UpdateUSBWhiltelist_And_FilterAllUSB();
        }
        #endregion


        #region Close()
        public static void Close()
        {
            AppServiceList.ForEach(app =>
            {
                app.Stop();
            });
        }
        #endregion
      
    }
}
