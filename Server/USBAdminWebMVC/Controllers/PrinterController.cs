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

        // site

        #region + public async Task<IActionResult> Site()
        public async Task<IActionResult> Site()
        {
            try
            {
                var vms = await _usbDb.IPPrinterSite_Get_ALL();
                
                return View(vms);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        #endregion

        #region + public async Task<IActionResult> SiteAddNew(Tbl_IPPrinterSite site)
        [HttpPost]
        public async Task<IActionResult> SiteAddNew(Tbl_IPPrinterSite site)
        {
            try
            {
                await _usbDb.IPPrinterSite_Insert(site);
                
                return Json(new { Msg = "Add a new site succeed." });
            }
            catch (Exception ex)
            {               
                return Json(new { Msg = ex.GetBaseException().Message });
            }
        }
        #endregion

        #region + public async Task<IActionResult> SiteEdit(Tbl_IPPrinterSite site)
        [HttpPost]
        public async Task<IActionResult> SiteEdit(Tbl_IPPrinterSite site)
        {
            try
            {
                await _usbDb.IPPrinterSite_Update(site);

                return Json(new { Msg = "Site update succeed." });
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.GetBaseException().Message });
            }
        }
        #endregion

        #region + public async Task<IActionResult> SiteDelete(int id)
        public async Task<IActionResult> SiteDelete(int id)
        {
            try
            {
                await _usbDb.IPPrinterSite_Delete_ById(id);
                return Json(new { msg = "Delete succeed." });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message });
            }
        }
        #endregion

        // ipprinter

        #region + public async Task<IActionResult> SiteAddIPPrinter(Tbl_IPPrinterInfo printer)
        public async Task<IActionResult> SiteAddIPPrinter(Tbl_IPPrinterInfo printer)
        {
            try
            {
                await _usbDb.IPPrinterInfo_Insert(printer);
                ViewBag.OK = "IPPrinterInfo update succeed.";
                return View("OK");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.GetBaseException().Message;
                return View("Error");
            }
        }
        #endregion

        #region MyRegion
        //public async Task<IActionResult> IPPrinter
        #endregion
    }
}
