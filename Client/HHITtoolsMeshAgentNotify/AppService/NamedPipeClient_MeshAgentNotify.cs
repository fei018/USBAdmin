using AgentLib;
using AgentLib.AppService;
using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HHITtoolsMeshAgentNotify
{
    public class NamedPipeClient_MeshAgentNotify:IAppService
    {
        // private
        private string _pipeName;

        private NamedPipeClient<string> _client;

        #region Construction
        public NamedPipeClient_MeshAgentNotify()
        {
            InitialPipeMsgHandler();
        }
        #endregion

        #region Stop()
        public void Stop()
        {
            try
            {
                if (_client != null)
                {
                    _client.ServerMessage -= Pipe_ReceiveMsg;
                    _client.Stop();
                    _client = null;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Start()
        public void Start()
        {
            try
            {
                Stop();

                _pipeName = AgentRegistry.AgentHttpKey;

                if (string.IsNullOrWhiteSpace(_pipeName))
                {
                    AgentLogger.Error("NamedPipeClient_MeshAgentNotify PipeName is empty");
                    return;
                }

                _client = new NamedPipeClient<string>(_pipeName);
                _client.AutoReconnect = true;

                _client.ServerMessage += Pipe_ReceiveMsg;

                _client.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
        #endregion

        #region Pipe_ReceiveMsg(NamedPipeConnection<string, string> connection, string usbJson)
        private void Pipe_ReceiveMsg(NamedPipeConnection<string, string> connection, string message)
        {
            try
            {
                //Debugger.Break();
                if (string.IsNullOrEmpty(message))
                {
                    throw new Exception("PipeClient_Tray.Pipe_ReceiveMsg(): message IsNullOrEmpty.");
                }

                var pipeMsg = JsonConvert.DeserializeObject<PipeMsg>(message);

                if (_pipeMsgHandler.ContainsKey(pipeMsg.PipeMsgType))
                {
                    _pipeMsgHandler[pipeMsg.PipeMsgType].Invoke(pipeMsg);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region InitialPipeMsgHandler()
        private Dictionary<PipeMsgType, Action<PipeMsg>> _pipeMsgHandler;

        private void InitialPipeMsgHandler()
        {
            _pipeMsgHandler = new Dictionary<PipeMsgType, Action<PipeMsg>>()
            {
                
            };
        }
        #endregion

        // Receive Msg From PipeServerAgent Handler

        #region ReceiveMsgHandler_Message_ShowMessageBox(PipeMsg pipeMsg)
        private void ReceiveMsgHandler_Message_ShowMessageBox(PipeMsg pipeMsg)
        {
            MessageBox.Show(pipeMsg.Message);
        }
        #endregion
        // send message

        #region + private void SendMsgToServer_By_PipeMsg(PipeMsg pipeMsg)
        private void SendMsgToServer_By_PipeMsg(PipeMsg pipeMsg)
        {
            try
            {
                var json = JsonConvert.SerializeObject(pipeMsg);
                _client.PushMessage(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void SendMsg_ToStartMeshAgentService()
        public void SendMsg_ToStartMeshAgentService()
        {
            try
            {
                var msg = new PipeMsg(PipeMsgType.ToStartMeshAgentService_ServerHandle);
                SendMsgToServer_By_PipeMsg(msg);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + public void SendMsg_ToStopMeshAgentService()
        public void SendMsg_ToStopMeshAgentService()
        {
            try
            {
                var msg = new PipeMsg(PipeMsgType.ToStopMeshAgentService_ServerHandle);
                SendMsgToServer_By_PipeMsg(msg);
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
