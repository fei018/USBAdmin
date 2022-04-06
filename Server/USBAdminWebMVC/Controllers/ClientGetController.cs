using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolsCommon;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    public class ClientGetController : Controller
    {
        private readonly USBAdminDatabaseHelp _usbDb;

        public ClientGetController(USBAdminDatabaseHelp usbDb)
        {
            _usbDb = usbDb;
        }


        // Usb Whitelist

        #region UsbWhitelist()
        public async Task<IActionResult> UsbWhitelist()
        {
            try
            {
                var query = await _usbDb.UsbWhitelist_Get();

                var agent = new AgentHttpResponseResult { Succeed = true, UsbWhitelist = query };

                return Json(agent);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

    }
}
