using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace USBNotifyLib
{
    public class PipeServerAgent
    {
        private string _pipeName;

        private NamedPipeServer<string> _server;

        // public static Entity
        public static PipeServerAgent Entity_Agent { get; set; }

        #region Event
        /// <summary>
        /// To Close Agent app event
        /// </summary>
        public event EventHandler CloseAgentAppEvent;
        #endregion

        #region Construction
        public PipeServerAgent()
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
                    AgentLogger.Error("PipeName is empty");
                    return;
                }

                Stop();

                PipeSecurity pipeSecurity = new PipeSecurity();

                pipeSecurity.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, AccessControlType.Allow));
                pipeSecurity.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, AccessControlType.Allow));

                // Allow Everyone read and write access to the pipe.
                pipeSecurity.AddAccessRule(
                            new PipeAccessRule(
                            "Authenticated Users",
                            PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance,
                            AccessControlType.Allow));

                _server = new NamedPipeServer<string>(_pipeName, pipeSecurity);

                _server.ClientMessage += ReceiveMsg_FromClientPipe;

                _server.Error += pipeConnection_Error;

                _server.Start();
            }
            catch (Exception)
            {
            }
        }

        private void pipeConnection_Error(Exception exception)
        {
            AgentLogger.Error("AgentPipe Error: " + exception.Message);
        }
        #endregion

        #region + public void Stop()
        public void Stop()
        {
            try
            {
                if (_server != null)
                {
                    _server.Error -= pipeConnection_Error;
                    _server.ClientMessage -= ReceiveMsg_FromClientPipe;
                    _server.Stop();
                    _server = null;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion


        // Receive message from client

        #region ReceiveMsg_FromClientPipe(NamedPipeConnection<string, string> connection, string message)
        private void ReceiveMsg_FromClientPipe(NamedPipeConnection<string, string> connection, string message)
        {
            try
            {
                //Debugger.Break();

                var pipeMsg = JsonConvert.DeserializeObject<PipeMsg>(message);

                if (pipeMsg == null)
                {
                    throw new Exception("AgentPipe : PipeMsg is Null");
                }

                if (_pipeMsgHandler.ContainsKey(pipeMsg.PipeMsgType))
                {
                    _pipeMsgHandler[pipeMsg.PipeMsgType].Invoke(pipeMsg);
                }

                return;

                #region switch
                //switch (pipeMsg.PipeMsgType)
                //{
                //    // check and update agent
                //    case PipeMsgType.UpdateAgent:
                //        Handler_UpdateAgent();
                //        break;

                //    // Update Setting and USB Whitelist
                //    case PipeMsgType.UpdateSetting:
                //        Handler_UpdateSetting();
                //        break;

                //    // To Close Agent
                //    case PipeMsgType.CloseAgent:
                //        Handler_CloseAgent();
                //        break;

                //    // To Close Tray
                //    case PipeMsgType.CloseTray:
                //        Handler_CloseTray();
                //        break;

                //    // Add Print Template
                //    case PipeMsgType.AddPrintTemplate:
                //        Handler_AddPrintTemplate();
                //        break;

                //    default:
                //        break;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion

        #region InitialPipeMsgHandler()
        private Dictionary<PipeMsgType, Action<PipeMsg>> _pipeMsgHandler;

        private void InitialPipeMsgHandler()
        {
            _pipeMsgHandler = new Dictionary<PipeMsgType, Action<PipeMsg>>()
            {
                { PipeMsgType.UpdateAgent, Handler_UpdateAgent },
                { PipeMsgType.UpdateSetting, Handler_UpdateSetting},
                { PipeMsgType.CloseAgent, Handler_CloseAgent },
                { PipeMsgType.CloseTray, Handler_CloseTray },
                { PipeMsgType.AddPrintTemplate, Handler_AddPrintTemplate }
            };
        }
        #endregion

        // Receive Message  handler

        #region + private void Handler_UpdateAgent(PipeMsg pipeMsg)
        private void Handler_UpdateAgent(PipeMsg pipeMsg)
        {
            Task.Run(() =>
            {
                try
                {
                    if (new AgentUpdate().CheckNeedUpdate())
                    {
                        new AgentUpdate().Update();
                        PushMsg_ToTray_Message("Download Agent done, wait for installation...");
                    }
                    else
                    {
                        PushMsg_ToTray_Message("Agent is newest version.");
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.GetBaseException().Message);
                    PushMsg_ToTray_Message(ex.GetBaseException().Message);
                }
            });
        }
        #endregion

        #region + private void Handler_UpdateSetting(PipeMsg pipeMsg)
        /// <summary>
        /// update AgentSetting and UsbWhitelist
        /// </summary>
        private void Handler_UpdateSetting(PipeMsg pipeMsg)
        {
            Task.Run(() =>
            {
                try
                {
                    new AgentHttpHelp().PostPerComputer_Http(); // post computer info

                    new AgentHttpHelp().GetAgentSetting_Http(); // update agent setting

                    new AgentHttpHelp().GetUsbWhitelist_Http(); // update usb whitelist

                    new UsbFilter().Filter_Scan_All_USB_Disk(); // filter all usb disk

                    AgentTimer.ReloadTask();                    // reload agent timer

                    PushMsg_ToTray_Message("Update Setting done.");
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.GetBaseException().Message);
                    PushMsg_ToTray_Message(ex.GetBaseException().Message);
                }
            });
        }
        #endregion

        #region + private void Handler_CloseAgent(PipeMsg pipeMsg)
        private void Handler_CloseAgent(PipeMsg pipeMsg)
        {
            try
            {
                CloseAgentAppEvent?.Invoke(this, null);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("AgentPipe.Handler_CloseAgent(): " + ex.GetBaseException().Message);
            }
        }
        #endregion

        #region + private void Handler_CloseTray(PipeMsg pipeMsg)
        private void Handler_CloseTray(PipeMsg pipeMsg)
        {
            try
            {
                PushMsg_ToTray_CloseTray();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region + private void Handler_AddPrintTemplate()
        private void Handler_AddPrintTemplate(PipeMsg pipeMsg)
        {
            Task.Run(() =>
            {
                //Debugger.Break();
                try
                {
                    var output = PrintTemplateHelp.Start(pipeMsg.PrintTemplateFile);

                    PushMsg_ToTray_AddPrintTemplateCompleted(output);
                }
                catch (Exception ex)
                {
                    PushMsg_ToTray_AddPrintTemplateCompleted(ex.GetBaseException().Message);
                    AgentLogger.Error(ex.GetBaseException().Message);
                }
            });
        }
        #endregion

        // push message

        #region + public void PushMsg_ToTray_Message(string message)
        public void PushMsg_ToTray_Message(string message)
        {
            try
            {
                var pipe = new PipeMsg(PipeMsgType.Message, message);
                var json = JsonConvert.SerializeObject(pipe);
                _server.PushMessage(json);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PushMessageToTray : " + ex.Message);
            }
        }
        #endregion

        #region + public void PushMsg_ToTray_UsbDiskNotInWhitelist(UsbDisk usb)
        public void PushMsg_ToTray_UsbDiskNotInWhitelist(UsbDisk usb)
        {
            try
            {
                if (_server == null) throw new Exception("NamedPipeServer is null.");

                if (usb != null)
                {
                    var pipeMsg = new PipeMsg(usb);
                    var msgJson = JsonConvert.SerializeObject(pipeMsg);
                    _server.PushMessage(msgJson);
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PushMsg_ToTray_UsbDiskNotInWhitelist(UsbDisk usb) : " + ex.Message);
            }
        }
        #endregion

        #region + public void PushMsg_ToTray_CloseTray()
        public void PushMsg_ToTray_CloseTray()
        {
            try
            {
                var pipe = new PipeMsg(PipeMsgType.CloseTray);
                var json = JsonConvert.SerializeObject(pipe);
                _server?.PushMessage(json);
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PushMessageToCloseTray : " + ex.Message);
            }
        }
        #endregion

        #region + public void PushMsg_ToTray_AddPrintTemplateCompleted()
        public void PushMsg_ToTray_AddPrintTemplateCompleted(string msg)
        {
            try
            {
                var pipe = new PipeMsg(PipeMsgType.AddPrintTemplateCompleted, msg);
                var json = JsonConvert.SerializeObject(pipe);
                _server.PushMessage(json);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PushMsg_ToTray_AddPrintTemplateCompleted : " + ex.Message);
            }
        }
        #endregion
    }
}
