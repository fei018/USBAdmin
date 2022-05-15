using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.ServiceProcess;

namespace HHITtoolsMeshAgentNotify
{
    class MeshAgentServiceHelp
    {
        private const string _agentServiceName = "Mesh Agent";

        private ManagementEventWatcher _watcher;

        public event EventHandler<MeshAgentServiceState> AgentServiceStateChangeEvent;

        #region + public void Start()
        public void Start()
        {
            try
            {
                if (!ServiceController.GetServices().Any(s => s.ServiceName == _agentServiceName))
                {
                    AgentServiceStateChangeEvent?.Invoke(null, MeshAgentServiceState.NoService);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent",
                                    new TimeSpan(0, 0, 1),
                                    $"TargetInstance isa \"Win32_Service\" and TargetInstance.Name = \"{_agentServiceName}\"");

                _watcher = new ManagementEventWatcher(query);

                _watcher.EventArrived += _watcher_EventArrived;
                _watcher.Start();
            }
            catch (Exception)
            {
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
