using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgentLib;
using AgentLib.AppService;
using AgentLib.Win32API;

namespace HHITtoolsService
{
    public class AppManager
    {
        public static List<IAppService> AppServicesList => new List<IAppService>();

        //

        #region Start()
        public void Start()
        {
            // post computer info to server
            new AgentHttpHelp().PostPerComputer_Http();

            // PipeServer_Service
            IAppService namedPipeService = new NamedPipeServer_Service();
            namedPipeService.Start();
            AppServicesList.Add(namedPipeService);

            // HHITtoolsUSB
            if (AgentRegistry.UsbFilterEnabled)
            {
                IAppService appUsb = new HHITtoolsUSBService();
                appUsb.Start();
                AppServicesList.Add(appUsb);
            }

            // PrintJobNotify
            try
            {
                if (AgentRegistry.PrintJobLogEnabled)
                {
                    
                }
            }
            catch (Exception) { }

            // HHITtoolsTray
            IAppService appTray = new HHITtoolsTrayService();
            appTray.Start();
            AppServicesList.Add(appTray);


            // ServiceTimer
            
        }
        #endregion

        #region Stop()
        public void Stop()
        {
            try
            {
                AppServicesList.ForEach(service =>
                {
                    service.Stop();
                });
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
