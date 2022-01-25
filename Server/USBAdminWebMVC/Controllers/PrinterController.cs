using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    public class PrinterController : Controller
    {
        private readonly USBAdminDatabaseHelp _usbDb;

        public PrinterController(USBAdminDatabaseHelp usbDbHelp)
        {
            _usbDb = usbDbHelp;
        }

        // Template

        #region + public IActionResult Template()
        public IActionResult Template()
        {
            return View();
        }
        #endregion

        #region + public async Task<IActionResult> TemplateList(int page, int limit)
        public async Task<IActionResult> TemplateList(int page, int limit)
        {
            try
            {
                var (total, list) = await _usbDb.PrintTemplate_Get_All(page, limit);
                return JsonResultHelp.LayuiTableData(total, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region + public IActionResult TemplateEdit(int? Id)
        public IActionResult TemplateEdit(int? Id)
        {
            if (Id.HasValue && Id.Value > 0)
            {
                var query = _usbDb.PrintTemplate_Get_ById(Id.Value).Result;
                return View(query);
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region + public async Task<IActionResult> TemplateEdit(Tbl_PrintTemplate template)
        /// <summary>
        /// Add new or update
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TemplateEdit(Tbl_PrintTemplate template)
        {
            try
            {
                
                if (template.Id <= 0)
                {
                    // insert new 
                    await _usbDb.PrintTemplate_Insert(template);
                }
                else
                {
                    // update
                    await _usbDb.PrintTemplate_Update(template);
                }

                ViewBag.OK = "Action succeed.";
                return View("OK");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        #endregion

        #region + public async Task<IActionResult> TemplateDelete(int id)
        public async Task<IActionResult> TemplateDelete(int id)
        {
            try
            {
                await _usbDb.PrinterTemplate_Delete_ById(id);
                return Json(new { msg = "Delete succeed." });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message });
            }
        }
        #endregion
    }
}
