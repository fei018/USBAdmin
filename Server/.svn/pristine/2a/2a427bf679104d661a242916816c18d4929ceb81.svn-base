using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    public class AgentSettingController : Controller
    {
        private readonly USBAdminDatabaseHelp _usbDb;


        public AgentSettingController(USBAdminDatabaseHelp usbDb)
        {
            _usbDb = usbDb;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var query = await _usbDb.AgentSetting_Get();
                return View(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSetting(Tbl_AgentSetting setting)
        {
            try
            {
                await _usbDb.AgentSetting_Update(setting);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
