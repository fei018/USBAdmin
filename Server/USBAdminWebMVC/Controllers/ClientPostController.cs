using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsCommon;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    public class ClientPostController : Controller
    {
        private readonly HttpContext _httpContext;

        private readonly USBDBHelp _usbDb;

        private readonly EmailHelp _email;

        public ClientPostController(IHttpContextAccessor httpContextAccessor, USBDBHelp usbDb, EmailHelp emailHelp)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _usbDb = usbDb;
            _email = emailHelp;
        }

        #region + private async Task<string> ReadHttpRequestBodyAsync()
        private async Task<string> ReadHttpRequestBodyAsync()
        {
            using StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
            return await body.ReadToEndAsync();
        }
        #endregion

        // Computer

        #region PostComputerInfo()
        [HttpPost]
        public async Task<IActionResult> PostComputerInfo()
        {
            try
            {               
                var comjosn = await ReadHttpRequestBodyAsync();

                var com = JsonHttpConvert.Deserialize_ComputerInfo(comjosn);
                await _usbDb.ComputerInfo_InsertOrUpdate(com);

                return Json(new AgentHttpResponseResult());
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // UsbHisory

        #region PostUsbLog()
        [HttpPost]
        public async Task<IActionResult> PostUsbLog()
        {
            try
            {
                using StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
                var post = await body.ReadToEndAsync();

                var info = JsonHttpConvert.Deserialize_UsbLog(post);

                await _usbDb.UsbLog_Insert(info);

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
                using StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
                var post = await body.ReadToEndAsync();

                Tbl_UsbRequest userPost_UsbRequest = JsonHttpConvert.Deserialize_UsbRequest(post);

                var usbInDb = await _usbDb.UsbRequest_Insert(userPost_UsbRequest);

                var com = await _usbDb.ComputerInfo_Get_ByIdentity(usbInDb.RequestComputerIdentity);

                await _email.SendToUser_UsbRequest_SubmitForm_Received(usbInDb, com);

                return Json(new AgentHttpResponseResult());
            }
            catch (EmailException ex)
            {
                return Json(new AgentHttpResponseResult(false, "Email notification error, please notify your IT Admin.\r\n" + ex.Message));
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // PrintJobLog

        #region PostPrintJobLog()
        public async Task<IActionResult> PostPrintJobLog()
        {
            try
            {
                using StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
                var post = await body.ReadToEndAsync();

                Tbl_PrintJobLog printJob = JsonHttpConvert.Deserialize_IPrintJobInfo(post);

                await _usbDb.PerPrintJob_Insert(printJob);

                return Json(new AgentHttpResponseResult());
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.GetBaseException().Message));
            }
        }
        #endregion
    }
}
