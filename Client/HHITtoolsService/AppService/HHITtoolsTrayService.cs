using AgentLib.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentLib;
using System.Diagnostics;
using System.Threading;

namespace HHITtoolsService
{
    public class HHITtoolsTrayService : IAppService
    {
        public Process AppProcess { get; private set; }

        public string AppFullPath { get; private set; }

        public void Start()
        {
            try
            {
                try
                {
                    AppFullPath = AgentRegistry.HHITtoolsTrayPath;
                }
                catch (Exception ex)
                {
                    throw new Exception("AgentRegistry.HHITtoolsTrayApp : " + ex.Message);
                }

                try
                {
                    AppProcess = AppProcessHelp.StartupAppAsLogonUser(AppFullPath);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error("HHITtoolsTrayService.Start(): " + ex.GetBaseException().Message);
            }
        }

        public void Stop()
        {
            try
            {
                AppService.NamedPipeServer.SendMsg_ToCloseProcess_HHITtoolsTray();
                Thread.Sleep(2000);
                AppProcessHelp.CloseOrKillProcess(AppProcess);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("HHITtoolsTrayService.Stop(): " + ex.GetBaseException().Message);
            }
        }
    }
}
