using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    public class AgentConfigController : Controller
    {
        private readonly USBDBHelp _usbDb;


        public AgentConfigController(USBDBHelp usbDb)
        {
            _usbDb = usbDb;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var query = await _usbDb.AgentConfig_Get();
                return View(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConfig(Tbl_AgentConfig setting)
        {
            try
            {
                await _usbDb.AgentConfig_Update(setting);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
