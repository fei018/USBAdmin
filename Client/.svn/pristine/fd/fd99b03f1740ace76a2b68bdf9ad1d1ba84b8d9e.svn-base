using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBNotifyLib
{
    public class PipeClientService
    {
        private static string PipeName = AgentRegistry.AgentHttpKey;

        private NamedPipeClient<string> _client;

        #region Start()
        public void Stop()
        {
            try
            {
                if (_client != null)
                {
                    _client.Error -= pipeConnection_Error;
                    _client.Stop();
                    _client = null;
                }
            }
            catch (Exception)
            {
            }
        }

        public void Start()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PipeName))
                {
                    AgentLogger.Error("usbnservice: PipeName is empty");
                    return;
                }

                Stop();

                _client = new NamedPipeClient<string>(PipeName);
                _client.AutoReconnect = true;

                _client.Error += pipeConnection_Error;

                _client.Start();
            }
            catch (Exception ex)
            {
                AgentLogger.Error("usbnservice: " + ex.Message);
            }
        }

        private void pipeConnection_Error(Exception ex)
        {
            AgentLogger.Error("usbnservice: " + ex.Message);
        }
        #endregion

        // push message

        #region + public void PushMsg_ToAgent_CloseAgent()
        public void PushMsg_ToAgent_CloseAgent()
        {
            try
            {
                var json = JsonConvert.SerializeObject(new PipeMsg(PipeMsgType.CloseAgent));
                _client?.PushMessage(json);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PushMessage_ToClose_Agent(): " + ex.Message);
            }
        }
        #endregion

        #region + public void PushMsg_ToAgent_CloseTray()
        public void PushMsg_ToAgent_CloseTray()
        {
            try
            {
                var json = JsonConvert.SerializeObject(new PipeMsg(PipeMsgType.CloseTray));
                _client?.PushMessage(json);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PushMessage_ToClose_Tray(): " + ex.Message);
            }
        }
        #endregion
    }
}
