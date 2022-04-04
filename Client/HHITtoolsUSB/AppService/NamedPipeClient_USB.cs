using AgentLib;
using AgentLib.AppService;
using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HHITtoolsUSB
{
    public class NamedPipeClient_USB : IAppService
    {
        private string _pipeName;

        private NamedPipeClient<string> _client;


        #region Construction
        public NamedPipeClient_USB()
        {
            _pipeName = AgentRegistry.AgentHttpKey;
            InitialPipeMsgHandler();
        }
        #endregion

        #region + public void Start()
        public void Start()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_pipeName))
                {
                    AgentLogger.Error("PipeClient_USB.Start(): PipeName is empty");
                    return;
                }

                Stop();

                _client = new NamedPipeClient<string>(_pipeName);

                _client.ServerMessage += Pipe_ReceiveMsg;

                _client.Error += pipeConnection_Error;

                _client.Start();
            }
            catch (Exception)
            {
            }
        }

        private void pipeConnection_Error(Exception exception)
        {
            AgentLogger.Error("PipeClient_USB.pipeConnection_Error(): " + exception.Message);
        }
        #endregion

        #region + public void Stop()
        public void Stop()
        {
            try
            {
                if (_client != null)
                {
                    _client.Error -= pipeConnection_Error;
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

        #region + private void Pipe_ReceiveMsg(NamedPipeConnection<string, string> connection, string message)
        private void Pipe_ReceiveMsg(NamedPipeConnection<string, string> connection, string message)
        {
            try
            {
                //Debugger.Break();

                if (string.IsNullOrEmpty(message))
                {
                    throw new Exception("PipeClient_USB.Pipe_ReceiveMsg(): message IsNullOrEmpty.");
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
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        #region InitialPipeMsgHandler
        private Dictionary<PipeMsgType, Action<PipeMsg>> _pipeMsgHandler;

        private void InitialPipeMsgHandler()
        {
            _pipeMsgHandler = new Dictionary<PipeMsgType, Action<PipeMsg>>()
            {
                { PipeMsgType.CloseHHITtoolsUSB_USBAppHandle, ReceiveMsgHandler_CloseHHITtoolsUSBApp },
            };
        }
        #endregion

        // ReceiveMsgHandler

        #region + private void ReceiveMsgHandler_CloseHHITtoolsUSBApp(PipeMsg pipeMsg)
        private void ReceiveMsgHandler_CloseHHITtoolsUSBApp(PipeMsg pipeMsg)
        {
            HHITtoolsUSBForm.HHToolsUSBForm.Close();
        }
        #endregion

        // PushMsg
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

        #region + public void SendMsgToTray_USBDiskNoRegister(UsbDisk usbDisk)
        public void SendMsgToTray_USBDiskNoRegister(UsbDisk usbDisk)
        {
            try
            {
                var msg = new PipeMsg(usbDisk);
                SendMsgToServer_By_PipeMsg(msg);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PipeClient_USB.SendMsgToTray_USBDiskNoRegister(): " + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}
