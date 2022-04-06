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

        #region Start()
        public static void Start()
        {
            // post computer info to server
            new AgentHttpHelp().PostComputerInfo_Http();

            // PipeServer_Service
            AppService.NamedPipeServer = new NamedPipeServer_Service();
            AppService.NamedPipeServer.Start();

            // HHITtoolsUSB
            if (AgentRegistry.UsbFilterEnabled)
            {
                AppService.HHITtoolsUSB = new HHITtoolsUSBService();
                AppService.HHITtoolsUSB.Start();
            }

            // PrintJobLog
            try
            {
                //if (AgentRegistry.PrintJobLogEnabled)
                //{
                //    AppService.PrintJobLogService = new PrintJobLog();
                //    AppService.PrintJobLogService.Start();
                //}
            }
            catch (Exception) { }


            // ServiceTimer
            AppService.ServiceTimer = new ServiceTimer();
            AppService.ServiceTimer.Start();
        }
        #endregion

        #region Stop()
        public static void Stop()
        {
            try
            {
                AppService.HHITtoolsUSB?.Stop();
            }
            catch (Exception) { }

            try
            {
                AppService.PrintJobLogService?.Stop();
            }
            catch (Exception) { }

            try
            {
                AppService.HHITtoolsTrayList.ForEach(t => t?.Stop());
            }
            catch (Exception) { }

            try
            {
                AppService.NamedPipeServer?.Stop();
            }
            catch (Exception) { }
        }
        #endregion

    }
}
