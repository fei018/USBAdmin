using AgentLib;
using AgentLib.AppService;
using AgentLib.Win32API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Management;

namespace HHITtoolsService
{
    public class MeshAgentServiceMonitor : IAppService
    {
        private const string _agentServiceName = "Mesh Agent";

        private TimeSpan _interval = new TimeSpan(0, 0, 5);

        private Timer _timer;

        #region + public void Start()
        public void Start()
        {           
            try
            {
                SetAgentServiceStartMode_Manual();

                Stop();

                _timer = new Timer();
                _timer.Interval = _interval.TotalMilliseconds;
                _timer.Elapsed += Elapsed_CheckAgent;
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                AgentLogger.Error("MeshAgentServiceMonitor.Start(): " + ex.Message);
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
                    _timer.Stop();
                    _timer.Elapsed -= Elapsed_CheckAgent;
                    _timer.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + private void Elapsed_CheckAgent()
        /// <summary>
        /// Task for Loop per 5s
        /// </summary>
        private void Elapsed_CheckAgent(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            try
            {
                if (!ServiceController.GetServices().Any(s => s.ServiceName.ToLower() == _agentServiceName.ToLower()))
                {
                    return;
                }
                
                // 如果 遠程控制中, 顯示當前用戶 notify window
                if (AgentRegistry.IsRemoteControl)
                {
                    if (AgentServiceIsRunning())
                    {
                        OpenCurrentUserRemoteNotifyWin();
                    }
                }
                else
                {
                    // 如果沒有遠程控制, mesh agent service to stop if it is running
                    using (ServiceController sc = new ServiceController(_agentServiceName))
                    {
                        if (sc.Status == ServiceControllerStatus.Running)
                        {
                            sc.Stop();
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                    }
                }
            }
            catch (Exception) {}
            finally
            {
                _timer.Enabled = true;
            }           
        }
        #endregion

        #region + private bool AgentServiceIsRunning()
        private bool AgentServiceIsRunning()
        {
            try
            {               
                using (ServiceController agentService = new ServiceController(_agentServiceName))
                {
                    if (agentService.Status == ServiceControllerStatus.Running)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion       

        #region + private void OpenCurrentUserRemoteNotifyWin()
        private void OpenCurrentUserRemoteNotifyWin()
        {
            int userSessionId = 0;

            // if System SessionId to return
            userSessionId = ProcessApiHelp.GetCurrentUserSessionID();
            if (userSessionId <= 0)
            {
                return;
            }

            try
            {
                string appPath = AgentRegistry.HHITtoolsMeshAgentNotifyPath;
                string appName = System.IO.Path.GetFileNameWithoutExtension(appPath);

                IEnumerable<Process> notifyWins = Process.GetProcessesByName(appName);

                if (notifyWins == null || notifyWins.Count() <= 0)
                {
                    AppProcessHelp.StartupAppAsLogonUser(appPath);
                }
                else
                {
                    // 如果 current user session 沒有, 則啓動 remote notify win
                    if (!notifyWins.Any(n => n.SessionId == userSessionId))
                    {
                        AppProcessHelp.StartupAppAsLogonUser(appPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MeshAgentServiceMonitor.OpenCurrentUserRemoteNotifyWin(): " + ex.Message);
            }
        }
        #endregion

        #region + private void SetAgentServiceStartMode_Manual()
        private void SetAgentServiceStartMode_Manual()
        {
            try
            {
                if (!ServiceController.GetServices().Any(s=>s.ServiceName == _agentServiceName))
                {
                    return;
                }

                ManagementObject ser = new ManagementObject(@"\root\cimv2", $"Win32_Service.Name='{_agentServiceName}'", null);

                ManagementBaseObject inParams = ser.GetMethodParameters("ChangeStartMode");
                inParams["StartMode"] = "Manual";

                ManagementBaseObject result = ser.InvokeMethod("ChangeStartMode", inParams, null);
                //string r = result["ReturnValue"] as string;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // public

        #region + public void StopMeshAgentService()
        public void StopMeshAgentService()
        {
            try
            {
                using (ServiceController sc = new ServiceController(_agentServiceName))
                {
                    sc.Stop();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + public void StartMeshAgentService()
        public void StartMeshAgentService()
        {
            try
            {
                using (ServiceController sc = new ServiceController(_agentServiceName))
                {
                    sc.Start();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
