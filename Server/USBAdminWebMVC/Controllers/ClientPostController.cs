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

        private readonly USBAdminDatabaseHelp _usbDb;

        public ClientPostController(IHttpContextAccessor httpContextAccessor, USBAdminDatabaseHelp usbDb)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _usbDb = usbDb;
        }

        #region + private async Task<string> ReadHttpRequestBodyAsync()
        private async Task<string> ReadHttpRequestBodyAsync()
        {
            using StreamReader body = new StreamReader(_httpContext.Request.Body, Encoding.UTF8);
            return await body.ReadToEndAsync();
        }
        #endregion

        // Computer

        #region PostPerComputer()
        [HttpPost]
        public async Task<IActionResult> PostPerComputer()
        {
            try
            {               
                var comjosn = await ReadHttpRequestBodyAsync();

                var com = JsonHttpConvert.Deserialize_PerComputer(comjosn);
                await _usbDb.PerComputer_InsertOrUpdate(com);

                return Json(new AgentHttpResponseResult());
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion
    }
}
