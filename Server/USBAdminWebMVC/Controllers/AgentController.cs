﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USBModel;
using USBCommon;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace USBAdminWebMVC.Controllers
{
    [AgentHttpKeyFilter]
    public class AgentController : Controller
    {
        private readonly USBAdminDatabaseHelp _usbDb;
        private readonly HttpContext _httpContext;
        private readonly EmailHelp _email;

        public AgentController(IHttpContextAccessor httpContextAccessor, USBAdminDatabaseHelp usbDb, EmailHelp emailHelp)
        {
            _usbDb = usbDb;
            _httpContext = httpContextAccessor.HttpContext;
            _email = emailHelp;
        }

        // Usb Whitelist

        #region UsbWhitelist()
        public async Task<IActionResult> UsbWhitelist()
        {
            try
            {
                var query = await _usbDb.UsbWhitelist_Get();

                var agent = new AgentHttpResponseResult { Succeed = true, UsbFilterData = query };

                return Json(agent);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // AgentSetting

        #region AgentSetting()
        public async Task<IActionResult> AgentSetting()
        {
            try
            {
                IAgentSetting query = await _usbDb.Get_AgentSetting();
                var agent = new AgentHttpResponseResult { Succeed = true, AgentSetting = query };

                return Json(agent);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false,ex.Message));
            }
        }
        #endregion

        // Computer

        #region PostPerComputer()
        [HttpPost]
        public async Task<IActionResult> PostPerComputer()
        {
            try
            {
                StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
                var comjosn = await body.ReadToEndAsync();

                var com = JsonHttpConvert.Deserialize_PerComputer(comjosn);
                await _usbDb.PerComputer_InsertOrUpdate(com);

                return Json(new AgentHttpResponseResult());
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false,ex.Message));
            }
        }
        #endregion

        // UsbHisory

        #region PostPerUsbHistory()
        [HttpPost]
        public async Task<IActionResult> PostPerUsbHistory()
        {
            try
            {
                StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
                var post = await body.ReadToEndAsync();

                var info = JsonHttpConvert.Deserialize_PerUsbHistory(post);

                await _usbDb.Insert_UsbHistory(info);

                return Json(new AgentHttpResponseResult());
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // UsbRequest

        #region PostUsbRequest()
        [HttpPost]      
        public async Task<IActionResult> PostUsbRequest()
        {
            try
            {
                //Debugger.Break();

                StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
                var post = await body.ReadToEndAsync();

                Tbl_UsbRequest userPost_UsbRequest = JsonHttpConvert.Deserialize_UsbRequest(post);

                var usbInDb = await _usbDb.UsbRequest_Insert(userPost_UsbRequest);

                var com = await _usbDb.PerComputer_Get_ByIdentity(usbInDb.RequestComputerIdentity);

                await _email.Send_UsbRequest_Notify_Submit_ToUser(usbInDb, com);

                return Json(new AgentHttpResponseResult());
            }
            catch(EmailException)
            {
                return Json(new AgentHttpResponseResult(false,"Email notification error, please notify your IT support."));
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false,ex.Message));
            }
        }
        #endregion

        // PrintTemplate

        #region PrintTemplate(string SubnetAddr)
        public async Task<IActionResult> PrintTemplate(string SubnetAddr)
        {
            try
            {
                var template = await _usbDb.PrintTemplate_Get_BySubnetAddr(SubnetAddr);
                var result = new AgentHttpResponseResult { Succeed = true, PrintTemplate = template };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // AgentUpdate

        #region AgentUpdate()
        public async Task<IActionResult> AgentUpdate()
        {
            try
            {
                var fileInfo = new FileInfo(USBAdminHelp.AgentUpdateFilePath);
                if (!fileInfo.Exists)
                {
                    throw new Exception("Update File not exist.");
                }

                using FileStream fs = fileInfo.OpenRead();
                byte[] buff = new byte[fileInfo.Length];

                if (await fs.ReadAsync(buff, 0, buff.Length) <= 0)
                {
                    throw new Exception("Update File read buff fail.");
                }

                var result = new AgentHttpResponseResult(true);

                await Task.Run(() =>
                {
                    result.DownloadFileBase64 = Convert.ToBase64String(buff);
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion
    }
}
