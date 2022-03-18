using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using AgentLib;
using AgentLib.Win32API;

namespace HHITtoolsService
{
    public class ToolsServiceHelp
    {
        private bool _autoBootUsbAgent = true;
        private bool _autoBootUsbAgentTray = true;

        private PipeClientService _servicePipe;

        #region ServiceStart()
        public static void Start_Service()
        {
            _autoBootUsbAgent = true;
            StartProcess_Agent();

            _servicePipe = new PipeClientService();
            _servicePipe.Start();

            // 判斷當前 windows session 是否 user session
            var sessionid = ProcessApiHelp.GetCurrentUserSessionID();
            if (sessionid > 0)
            {
                _autoBootUsbAgentTray = true;
                StartProcess_AgentTray();
            }
        }
        #endregion

        #region ServiceStop()
        public static void Stop_Service()
        {
            _autoBootUsbAgentTray = false;
            _autoBootUsbAgent = false;

            _servicePipe.PushMsg_ToAgent_CloseTray();

            _servicePipe.PushMsg_ToAgent_CloseAgent();

            CloseProcess_AgentTray();
            
            CloseProcess_Agent();

            _servicePipe?.Stop();
        }
        #endregion

        #region Agent Process

        private string _agentPath = AgentRegistry.HHITtoolsUSBApp;

        private Process _AgentProcess;

        private void StartProcess_Agent()
        {
            CloseProcess_Agent();
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(_agentPath);
                _AgentProcess = new Process
                {
                    EnableRaisingEvents = true,
                    StartInfo = startInfo
                };

                // Exited Event 委託, 如果意外結束process, 可以自己啟動
                _AgentProcess.Exited += (s, e) =>
                {
                    if (_autoBootUsbAgent)
                    {
                        StartProcess_Agent();
                    }
                };

                _AgentProcess.Start();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }

        private void CloseProcess_Agent()
        {
            try
            {
                if (_AgentProcess != null && !_AgentProcess.HasExited)
                {
                    _AgentProcess?.CloseMainWindow();

                    if (_AgentProcess != null && !_AgentProcess.HasExited)
                    {
                        Thread.Sleep(new TimeSpan(0, 0, 2));
                        _AgentProcess?.Kill();
                    }

                    _AgentProcess?.Close();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region AgentTray Process
        private string _agentTrayPath = AgentRegistry.HHITtoolsTrayApp;

        private Process _agentTrayProcess;

        private void StartProcess_AgentTray()
        {
            CloseProcess_AgentTray();

            try
            {
                _agentTrayProcess = ProcessApiHelp.CreateProcessAsUser(_agentTrayPath, null);
                _agentTrayProcess.EnableRaisingEvents = true;

                // Exited Event 委託, 如果意外結束process, 可以自己啟動
                _agentTrayProcess.Exited += (s, e) =>
                {
                    if (_autoBootUsbAgentTray)
                    {
                        StartProcess_AgentTray();
                    }
                };
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }

        private void CloseProcess_AgentTray()
        {
            try
            {
                if (_agentTrayProcess != null && !_agentTrayProcess.HasExited)
                {
                    _agentTrayProcess?.CloseMainWindow();

                    if (_agentTrayProcess != null && !_agentTrayProcess.HasExited)
                    {
                        Thread.Sleep(new TimeSpan(0, 0, 2));
                        _agentTrayProcess?.Kill();
                    }

                    _agentTrayProcess?.Close();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

       
    }
}
