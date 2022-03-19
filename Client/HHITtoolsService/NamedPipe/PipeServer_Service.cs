﻿using AgentLib;
using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHITtoolsService
{
    public class PipeServer_Service
    {
        private string _pipeName;

        private NamedPipeServer<string> _server;

        #region Construction
        public PipeServer_Service()
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
                    AgentLogger.Error("PipeServer_Service.Start(): PipeName is empty");
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

                _server.ClientMessage += Pipe_ReceiveMsg;

                _server.Error += pipeConnection_Error;

                _server.Start();
            }
            catch (Exception)
            {
            }
        }

        private void pipeConnection_Error(Exception exception)
        {
            AgentLogger.Error("PipeServer_Service pipeConnection_Error(): " + exception.Message);
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
                    _server.ClientMessage -= Pipe_ReceiveMsg;
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
        private void Pipe_ReceiveMsg(NamedPipeConnection<string, string> connection, string message)
        {
            try
            {
                //Debugger.Break();

                if (string.IsNullOrEmpty(message))
                {
                    throw new Exception("PipeServer_Service.Pipe_ReceiveMsg(): message IsNullOrEmpty.");
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
                AgentLogger.Error("PipeServer_Service.Pipe_ReceiveMsg(): " + ex.Message);
            }
        }
        #endregion

        #region InitialPipeMsgHandler()
        private Dictionary<PipeMsgType, Action<PipeMsg>> _pipeMsgHandler;

        private void InitialPipeMsgHandler()
        {
            _pipeMsgHandler = new Dictionary<PipeMsgType, Action<PipeMsg>>()
            {
                { PipeMsgType.UpdateAgent, ReceiveMsgHandler_UpdateAgent },
                { PipeMsgType.UpdateSetting, ReceiveMsgHandler_UpdateSetting},
                { PipeMsgType.UsbDiskNoRegister_USBToTray, ReceiveMsgHandler_UsbDiskNoRegister },
                { PipeMsgType.DeleteOldPrintersAndInstallDriver, Handler_PrinterDeleteOldAndInstallDriver },
                { PipeMsgType.PrintJobNotifyRestart, Handler_PrintJobNotifyRestart }
            };
        }
        #endregion

        // Receive Message  handler

        #region + private void ReceiveMsgHandler_UsbDiskNoRegister(PipeMsg pipeMsg)
        private void ReceiveMsgHandler_UsbDiskNoRegister(PipeMsg pipeMsg)
        {
            try
            {
                SendMsgToClient_By_PipeMsg(pipeMsg);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PipeServer_Service.ReceiveMsgHandler_UsbDiskNoRegister(): " + ex.Message);
            }
        }
        #endregion

        #region + private void ReceiveMsgHandler_UpdateAgent(PipeMsg pipeMsg)
        private void ReceiveMsgHandler_UpdateAgent(PipeMsg pipeMsg)
        {
            Task.Run(() =>
            {
                try
                {
                    if (new AgentUpdate().CheckNeedUpdate())
                    {
                        new AgentUpdate().Update();

                        var msg = new PipeMsg(PipeMsgType.Msg_ServerToTray, "Download Agent done, wait for installation...");
                        SendMsgToClient_By_PipeMsg(msg);
                    }
                    else
                    {
                        var msg = new PipeMsg(PipeMsgType.Msg_ServerToTray, "Agent is newest version.");
                        SendMsgToClient_By_PipeMsg(msg);
                    }
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.GetBaseException().Message);
                    var msg = new PipeMsg(PipeMsgType.Msg_ServerToTray, ex.GetBaseException().Message);
                    SendMsgToClient_By_PipeMsg(msg);
                }
            });
        }
        #endregion

        #region + private void ReceiveMsgHandler_UpdateSetting(PipeMsg pipeMsg)
        /// <summary>
        /// update AgentSetting and UsbWhitelist
        /// </summary>
        private void ReceiveMsgHandler_UpdateSetting(PipeMsg pipeMsg)
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

                    var msg = new PipeMsg(PipeMsgType.Msg_ServerToTray, "Update Setting done.");
                    SendMsgToClient_By_PipeMsg(msg);
                }
                catch (Exception ex)
                {
                    AgentLogger.Error(ex.GetBaseException().Message);
                    var msg = new PipeMsg(PipeMsgType.Msg_ServerToTray, ex.GetBaseException().Message);
                    SendMsgToClient_By_PipeMsg(msg);
                }
            });
        }
        #endregion

        #region + private void Handler_PrinterDeleteOldAndInstallDriver(PipeMsg pipeMsg)
        private void Handler_PrinterDeleteOldAndInstallDriver(PipeMsg pipeMsg)
        {
            Task.Run(() =>
            {
#if DEBUG
                Debugger.Break();
#endif
                try
                {
                    pipeMsg.PipeMsgType = PipeMsgType.DeleteOldPrintersAndInstallDriverCompleted;

                    // delete old subnet printer
                    PrinterHelp.DeleteOldIPPrinters_OtherSubnet();

                    // delete old same name printer
                    foreach (var p in pipeMsg.SitePrinterToAddList.PrinterList)
                    {
                        try
                        {
                            PrinterHelp.DeletePrinter_ByName(p.PrinterName);
                            PrinterHelp.DeleteTcpIPPort_ByName(p.PortIPAddr);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    StringBuilder output = new StringBuilder();

                    //// add driver
                    var driverList = pipeMsg.SitePrinterToAddList.DriverList;
                    if (driverList != null && driverList.Any())
                    {
                        foreach (var p in driverList)
                        {                           
                            try
                            {
                                PrinterHelp.InstallPrinterDriver_WMI(p.DriverName, p.DriverInfLocalPath);
                                output.AppendLine("Add printer driver: " + p.DriverName);
                            }
                            catch (Exception ex)
                            {
                                output.AppendLine(ex.GetBaseException().Message);
                            }
                        }
                    }

                    pipeMsg.Message = pipeMsg.Message + "\r\n" + output.ToString() + "\r\n";
                    SendMsgToClient_By_PipeMsg(pipeMsg);
                }
                catch (Exception ex)
                {

                    pipeMsg.Message = pipeMsg.Message + "\r\n" + ex.GetBaseException().Message + "\r\n";
                    SendMsgToClient_By_PipeMsg(pipeMsg);
                }
            });
        }
        #endregion

        #region + private void Handler_PrintJobNotifyRestart(PipeMsg pipeMsg)
        private void Handler_PrintJobNotifyRestart(PipeMsg pipeMsg)
        {
            try
            {
                PrintJobNotify.Stop();
                PrintJobNotify.Start();
            }
            catch (Exception ex)
            {
                AgentLogger.Error("Handler_PrintJobNotifyRestart: " + ex.Message);
            }
        }
        #endregion

        // Sned message func

        #region + private void SendMsgToClient_By_PipeMsg(PipeMsg pipeMsg)
        private void SendMsgToClient_By_PipeMsg(PipeMsg pipeMsg)
        {
            try
            {
                var json = JsonConvert.SerializeObject(pipeMsg);
                _server.PushMessage(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void SendMsg_CloseHHITtoolsTray()
        public void SendMsg_CloseHHITtoolsTray()
        {
            try
            {
                var msg = new PipeMsg(PipeMsgType.CloseHHITtoolsTray);

                SendMsgToClient_By_PipeMsg(msg);
            }
            catch (Exception ex)
            {
                AgentLogger.Error("PipeServer_Service.SendMsg_CloseHHITtoolsTray() : " + ex.Message);
            }
        }
        #endregion

        #region + public void SendMsg_CloseHHITtoolsUSB()
        public void SendMsg_CloseHHITtoolsUSB()
        {
            try
            {
                var msg = new PipeMsg(PipeMsgType.CloseHHITtoolsUSB);
                SendMsgToClient_By_PipeMsg(msg);
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
            }
        }
        #endregion
    }
}