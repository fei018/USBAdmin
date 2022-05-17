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
            //--- post computer info to server
            try
            {
                new AgentHttpHelp().PostComputerInfo();
            }
            catch (Exception ex) { AgentLogger.Error("HHITtoolsService.AppManager.Start(): " + ex.Message); }

            //--- PipeServer_Service
            try
            {
                AppService.NamedPipeServer = new NamedPipeServer_Service();
                AppService.NamedPipeServer.Start();
            }
            catch (Exception) { }

            //--- HHITtoolsUSB
            try
            {
                AppService.HHITtoolsUSB = new HHITtoolsUSBService();
                AppService.HHITtoolsUSB.Start();
            }
            catch (Exception)
            {
            }

            //--- Tray
            AppService.HHITtoolsTrayList = new List<HHITtoolsTrayService>();

            var tray = new HHITtoolsTrayService();
            tray.Start();

            AppService.HHITtoolsTrayList.Add(tray);

            
            //--- ServiceTimer
            AppService.ServiceTimer = new ServiceTimer();
            AppService.ServiceTimer.Start();

            //--- PrintJobLogService
            try
            {
                if (AgentRegistry.PrintJobLogEnabled)
                {
                    AppService.PrintJobLogService = new PrintJobLogService();
                    AppService.PrintJobLogService.Start();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Stop()
        public static void Stop()
        {
            //--- HHITtoolsUSB
            try
            {
                AppService.HHITtoolsUSB?.Stop();
            }
            catch (Exception) { }

            //--- PrintJobLogService
            try
            {
                AppService.PrintJobLogService?.Stop();
            }
            catch (Exception) { }

            //--- HHITtoolsTrayList
            try
            {
                foreach (var tray in AppService.HHITtoolsTrayList)
                {
                    tray?.Stop();
                }
            }
            catch (Exception ex) { AgentLogger.Error(ex.GetBaseException().Message); }

            //--- ServiceTimer
            try
            {
                AppService.ServiceTimer?.Stop();
            }
            catch (Exception) { }

            //------- NamedPipeServer ------ last stop
            try
            {
                AppService.NamedPipeServer?.Stop();
            }
            catch (Exception) { }           
        }
        #endregion

    }
}
