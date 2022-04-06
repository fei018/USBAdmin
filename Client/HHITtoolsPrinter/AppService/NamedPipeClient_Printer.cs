using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AgentLib;
using AgentLib.AppService;
using NamedPipeWrapper;
using Newtonsoft.Json;
using ToolsCommon;

namespace HHITtoolsPrinter
{
    class NamedPipeClient_Printer : IAppService
    {
        private string _pipeName;

        private NamedPipeClient<string> _client;

        #region Event

        public event EventHandler<PipeEventArgs> AddSitePrinterCompletedEvent;
        #endregion

        #region Construction
        public NamedPipeClient_Printer()
        {            
            InitialPipeMsgHandler();
        }
        #endregion

        #region private void SendPipeMsgToServer_Agent(PipeMsg pipeMsg)
        private void SendPipeMsgToServer_Agent(PipeMsg pipeMsg)
        {
            try
            {
                var json = JsonConvert.SerializeObject(pipeMsg);
                _client?.PushMessage(json);
            }
            catch (Exception)
            {
                throw;
            }
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
                _pipeName = AgentRegistry.AgentHttpKey;

                if (string.IsNullOrWhiteSpace(_pipeName))
                {
                    AgentLogger.Error("PipeName is empty");
                    return;
                }

                Stop();

                _client = new NamedPipeClient<string>(_pipeName);
                _client.AutoReconnect = true;

                _client.ServerMessage += Pipe_ReceiveMsg;

                _client.Start();
            }
            catch (Exception)
            {
                throw;
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
                { PipeMsgType.Msg_TrayHandle, ReceiveMsgHandler_Message_ShowMessageBox }
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

        #region Handler_FromAgentMsg_PrinterDeleteOldAndAddDriverCompleted(PipeMsg pipeMsg) 
        private void Handler_FromAgentMsg_PrinterDeleteOldAndAddDriverCompleted(PipeMsg pipeMsg)
        {
            Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    if (pipeMsg.PipeMsgType == PipeMsgType.Msg_TrayHandle)
                    {
                        throw new Exception(pipeMsg.Message);
                    }

                    sb.AppendLine(pipeMsg.Message);

                    // add printer
                    foreach (var sp in pipeMsg.SitePrinterToAddList.PrinterList)
                    {
                        try
                        {
                            PrinterHelp.AddNewPrinter(sp);
                            sb.AppendLine("Add Printer: " + sp.PrinterName);
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(ex.GetBaseException().Message);
                        }
                    }

                    AddSitePrinterCompletedEvent?.Invoke(null, new PipeEventArgs(sb.ToString()));
                }
                catch (Exception ex)
                {
                    sb.AppendLine().AppendLine(ex.GetBaseException().Message);
                    AddSitePrinterCompletedEvent?.Invoke(null, new PipeEventArgs(sb.ToString()));
                }

                try
                {
                    SendPipeMsgToServer_Agent(new PipeMsg(PipeMsgType.PrintJobLogRestart));
                }
                catch (Exception)
                {
                }
            });
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

        // 

        #region + public void PushMsg_ToAgent_SitePrinterToAdd()
        public void PushMsg_ToAgent_SitePrinterToAdd(List<IPPrinterInfo> sitePrinterList)
        {
            Task.Run(() =>
            {
                try
                {
                    // 獲取 需要 install driver list
                    List<IPPrinterInfo> addDriverList = new List<IPPrinterInfo>();
                    foreach (var addPrinter in sitePrinterList)
                    {
                        try
                        {
                            if (!PrinterHelp.PrinterDriverExist(addPrinter.DriverName))
                            {
                                // copy inf to local from unc path
                                var infFile = new FileInfo(addPrinter.DriverInfPath);
                                if (infFile.Exists)
                                {
                                    var infDir = infFile.Directory;
                                    string destDriverDir = Path.Combine(AgentRegistry.AgentDataDir, "PrinterDriver", infDir.Name);

                                    if (Directory.Exists(destDriverDir))
                                    {
                                        Directory.Delete(destDriverDir, true);
                                    }

                                    UtilityTools.CopyDirectory(infDir.FullName, destDriverDir, true);
                                    addPrinter.DriverInfLocalPath = Path.Combine(destDriverDir, infFile.Name);

                                    addDriverList.Add(addPrinter);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                    // to printer delete old and add driver by Agent

                    SitePrinterToAddList sitePrinterToAddList = new SitePrinterToAddList
                    {
                        PrinterList = sitePrinterList,
                        DriverList = addDriverList
                    };

                    // set PipeMsgType: PrinterDeleteOldAndInstallDriver
                    var pipemsg = new PipeMsg()
                    {
                        PipeMsgType = PipeMsgType.DeleteOldPrintersAndInstallDriver_ServerHandle,
                        SitePrinterToAddList = sitePrinterToAddList
                    };

                    SendPipeMsgToServer_Agent(pipemsg);
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }
        #endregion
    }
}
