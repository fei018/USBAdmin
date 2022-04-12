using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolsCommon;
using HHITtoolsTray.USBWindow;
using AgentLib;
using System.Diagnostics;
using AgentLib.AppService;

namespace HHITtoolsTray
{
    public class NamedPipeClient_Tray : IAppService
    {
        // private
        private string _pipeName;

        private NamedPipeClient<string> _client;

        #region Construction
        public NamedPipeClient_Tray()
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
                    AgentLogger.Error("NamedPipeClient_Tray PipeName is empty");
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
                { PipeMsgType.Msg_TrayHandle, ReceiveMsgHandler_Message_ShowMessageBox },
                { PipeMsgType.UsbDiskNoRegister_NotifyTray_TrayHandle, ReceiveMsgHandler_UsbDiskNoRegister},
                { PipeMsgType.ToCloseProcess_HHITtoolsTray_TrayHandle, ReceiveMsgHandler_ToCloseProcess_Tray }
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

        #region ReceiveMsgHandler_UsbDiskNoRegister(PipeMsg pipeMsg)
        private void ReceiveMsgHandler_UsbDiskNoRegister(PipeMsg pipeMsg)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    //Debugger.Break();
                    var usbWin = new UsbRequestWin();
                    usbWin.ShowPageUsbRequestNotify(pipeMsg.UsbDisk);
                    usbWin.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "USB Control");
                }
            }));
        }
        #endregion

        #region ReceiveMsgHandler_ToCloseProcess_Tray(PipeMsg pipeMsg)
        private void ReceiveMsgHandler_ToCloseProcess_Tray(PipeMsg pipeMsg)
        {
            try
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    App.Current.MainWindow.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        #region + public void SendMsg_CheckAndUpdateAgent()
        public void SendMsg_CheckAndUpdateAgent()
        {
            try
            {
                var msg = new PipeMsg(PipeMsgType.UpdateAgent_ServerHandle);
                SendMsgToServer_By_PipeMsg(msg);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + public void SendMsg_UpdateSetting()
        public void SendMsg_UpdateSetting()
        {
            try
            {
                var msg = new PipeMsg(PipeMsgType.UpdateSetting_ServerHandle);
                SendMsgToServer_By_PipeMsg(msg);
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
