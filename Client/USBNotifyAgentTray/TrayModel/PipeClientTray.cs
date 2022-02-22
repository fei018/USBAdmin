using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using USBCommon;
using USBNotifyAgentTray.USBWindow;
using USBNotifyLib;

namespace USBNotifyAgentTray
{
    public class PipeClientTray
    {
        // private
        private string _pipeName;

        private NamedPipeClient<string> _client;

        // public static
        public static PipeClientTray Entity_Tray { get; set; }

        #region Event
        public event EventHandler<PipeEventArgs> AddPrintTemplateCompletedEvent;

        public event EventHandler<PipeEventArgs> AddSitePrinterCompletedEvent;
        #endregion

        #region Construction
        public PipeClientTray()
        {
            _pipeName = AgentRegistry.AgentHttpKey;

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

        #region Start()
        public void Stop()
        {
            try
            {
                if (_client != null)
                {
                    _client.ServerMessage -= ReceiveMsg_FromPipeServerAgent;
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
                if (string.IsNullOrWhiteSpace(_pipeName))
                {
                    AgentLogger.Error("PipeName is empty");
                    return;
                }

                Stop();

                _client = new NamedPipeClient<string>(_pipeName);
                _client.AutoReconnect = true;

                _client.ServerMessage += ReceiveMsg_FromPipeServerAgent;

                _client.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ReceiveMsg_FromPipeServerAgent(NamedPipeConnection<string, string> connection, string usbJson)
        private void ReceiveMsg_FromPipeServerAgent(NamedPipeConnection<string, string> connection, string message)
        {
            try
            {
                //Debugger.Break();
                var pipeMsg = JsonConvert.DeserializeObject<PipeMsg>(message);

                if (pipeMsg == null)
                {
                    throw new Exception("TrayPipe: PipeMsg is Null.");
                }

                if (_pipeMsgHandler.ContainsKey(pipeMsg.PipeMsgType))
                {
                    _pipeMsgHandler[pipeMsg.PipeMsgType].Invoke(pipeMsg);
                }

                return;

                #region switch
                //switch (pipeMsg.PipeMsgType)
                //{
                //    case PipeMsgType.Message:
                //        Handler_FromAgentMsg_MessageBox(pipeMsg.Message);
                //        break;

                //    case PipeMsgType.UsbDiskNotInWhitelist:
                //        Handler_FromAgentMsg_UsbNotifyWindow(pipeMsg.UsbDisk);
                //        break;

                //    case PipeMsgType.CloseTray:
                //        Handler_FromAgentMsg_ToCloseTray();
                //        break;

                //    case PipeMsgType.AddPrintTemplateCompleted:
                //        Handler_FromAgentMsg_AddPrintTemplateCompleted(pipeMsg.Message);
                //        break;

                //    default:
                //        break;
                //}
                #endregion

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
                { PipeMsgType.Message, Handler_FromAgentMsg_MessageBox },
                { PipeMsgType.UsbDiskNotInWhitelist, Handler_FromAgentMsg_UsbNotifyWindow},
                { PipeMsgType.CloseTray, Handler_FromAgentMsg_ToCloseTray },
                { PipeMsgType.AddPrintTemplateCompleted, Handler_FromAgentMsg_AddPrintTemplateCompleted },
                { PipeMsgType.PrinterDeleteOldAndInstallDriverCompleted, Handler_FromAgentMsg_PrinterDeleteOldAndAddDriverCompleted }
            };
        }
        #endregion

        // Receive Msg From PipeServerAgent Handler

        #region Handler_FromAgentMsg_MessageBox(PipeMsg pipeMsg)
        private void Handler_FromAgentMsg_MessageBox(PipeMsg pipeMsg)
        {
            MessageBox.Show(pipeMsg.Message);
        }
        #endregion

        #region Handler_FromAgentMsg_UsbNotifyWindow(PipeMsg pipeMsg)
        private void Handler_FromAgentMsg_UsbNotifyWindow(PipeMsg pipeMsg)
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

        #region Handler_FromAgentMsg_ToCloseTray(PipeMsg pipeMsg)
        private void Handler_FromAgentMsg_ToCloseTray(PipeMsg pipeMsg)
        {
            try
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    App.Current.MainWindow.Close();
                }));
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
                Debug.WriteLine(ex.Message);
#endif
            }
        }
        #endregion

        #region Handler_FromAgentMsg_AddPrintTemplateCompleted(PipeMsg pipeMsg)
        private void Handler_FromAgentMsg_AddPrintTemplateCompleted(PipeMsg pipeMsg)
        {
            try
            {
                AddPrintTemplateCompletedEvent?.Invoke(null, new PipeEventArgs(pipeMsg.Message));
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
                Debug.WriteLine(ex.Message);
#endif
            }
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
                    if (pipeMsg.PipeMsgType == PipeMsgType.Message)
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
                    SendPipeMsgToServer_Agent(new PipeMsg(PipeMsgType.PrintJobNotifyRestart));
                }
                catch (Exception)
                {
                }
            });
        }
        #endregion

        // push message

        #region + public void PushMsg_ToAgent_CheckAndUpdateAgent()
        public void PushMsg_ToAgent_CheckAndUpdateAgent()
        {
            try
            {
                var json = JsonConvert.SerializeObject(new PipeMsg(PipeMsgType.UpdateAgent));
                _client?.PushMessage(json);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + public void PushMsg_ToAgent_UpdateSetting()
        public void PushMsg_ToAgent_UpdateSetting()
        {
            try
            {
                var json = JsonConvert.SerializeObject(new PipeMsg(PipeMsgType.UpdateSetting));
                _client?.PushMessage(json);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + public void PushMsg_ToAgent_AddPrintTemplate()
        public void PushMsg_ToAgent_AddPrintTemplate()
        {
            try
            {
#if DEBUG
                //Debugger.Break();
#endif
                // get template from http server 
                var template = new AgentHttpHelp().GetPrintTemplate_Http();

                //check FilePath(UNC) whether exist
                var templateFile = new FileInfo(template.FilePath?.Trim());
                if (!templateFile.Exists)
                {
                    throw new Exception("PrintTemplate file not exist.\r\nPath: " + template.FilePath);
                }

                // copy template file from unc to local data dir
                var localTemplate = PrintTemplateHelp.CopyTemplateFileToLocal(templateFile);

                var json = JsonConvert.SerializeObject(new PipeMsg(PipeMsgType.AddPrintTemplate) { PrintTemplateFile = localTemplate });
                _client?.PushMessage(json);
            }
            catch (Exception)
            {
                throw;
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
                        PipeMsgType = PipeMsgType.PrinterDeleteOldAndInstallDriver,
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
