using System;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Windows;

namespace HHITtoolsMeshAgentNotify
{
    public class MeshAgentServiceHelp
    {
        private const string _agentServiceName = "Mesh Agent";

        private ManagementEventWatcher _watcher;

        public event EventHandler<MeshAgentServiceState> AgentServiceStateChangeEvent;

        #region + public void Start()
        public void Start()
        {
          
            // 綁定 meshagent service state change event
            try
            {
                WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent",
                                    new TimeSpan(0, 0, 1),
                                    $"TargetInstance isa \"Win32_Service\" and TargetInstance.Name = \"{_agentServiceName}\"");

                _watcher = new ManagementEventWatcher(query);

                _watcher.EventArrived += _watcher_EventArrived;
                _watcher.Start();

                CheckServiceStateToInvokeEvent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
        #endregion

        #region + private void CheckServiceStateToInvokeEvent()
        private void CheckServiceStateToInvokeEvent()
        {
            try
            {
                if (!ServiceController.GetServices().Any(s => s.ServiceName == _agentServiceName))
                {
                    AgentServiceStateChangeEvent?.Invoke(null, MeshAgentServiceState.NoService);
                }
                else
                {
                    using (ServiceController sc = new ServiceController(_agentServiceName))
                    {
                        switch (sc.Status)
                        {
                            case ServiceControllerStatus.ContinuePending:
                                break;

                            case ServiceControllerStatus.Paused:
                                break;

                            case ServiceControllerStatus.PausePending:
                                break;

                            case ServiceControllerStatus.Running:
                                AgentServiceStateChangeEvent?.Invoke(null, MeshAgentServiceState.Running);
                                break;

                            case ServiceControllerStatus.StartPending:
                                break;

                            case ServiceControllerStatus.Stopped:
                                AgentServiceStateChangeEvent?.Invoke(null, MeshAgentServiceState.Stopped);
                                break;

                            case ServiceControllerStatus.StopPending:
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void Stop()
        public void Stop()
        {
            try
            {
                if (_watcher != null)
                {
                    _watcher.Stop();
                    _watcher.EventArrived -= _watcher_EventArrived;
                    _watcher.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + private void _watcher_EventArrived(object sender, EventArrivedEventArgs e)
        private void _watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject service = e.NewEvent["TargetInstance"] as ManagementBaseObject;
                string state = service.GetPropertyValue("State") as string;

                if (string.IsNullOrEmpty(state))
                {
                    return;
                }

                if (state.ToLower() == "running")
                {
                    AgentServiceStateChangeEvent?.Invoke(null, MeshAgentServiceState.Running);
                    return;
                }

                if (state.ToLower() == "stopped")
                {
                    AgentServiceStateChangeEvent?.Invoke(null, MeshAgentServiceState.Stopped);
                    return;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
