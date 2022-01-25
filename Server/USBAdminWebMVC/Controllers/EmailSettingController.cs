using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    public class EmailSettingController : Controller
    {
        private readonly USBAdminDatabaseHelp _usbDb;


        public EmailSettingController(USBAdminDatabaseHelp usbDb)
        {
            _usbDb = usbDb;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var query = await _usbDb.EmailSetting_Get();
                return View(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateSetting(Tbl_EmailSetting setting)
        {
            try
            {
                await _usbDb.EmailSetting_Update(setting);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
