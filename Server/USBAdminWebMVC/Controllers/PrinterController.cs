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
        private readonly USBDBHelp _usbDb;

        public PrinterController(USBDBHelp usbDbHelp)
        {
            _usbDb = usbDbHelp;
        }

        // site

        #region + public async Task<IActionResult> SiteIndex()
        public async Task<IActionResult> SiteIndex()
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

        #region SiteEdit
        public async Task<IActionResult> SiteEdit(string id)
        {
            try
            {
                var site = await _usbDb.IPPrinterSite_Get_ById(Guid.Parse(id));
                return View(site);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.GetBaseException().Message;
                return View("Error");
            }
        }

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

        #region + public async Task<IActionResult> SiteDelete(string id)
        [HttpPost]
        public async Task<IActionResult> SiteDelete(string id)
        {
            try
            {
                await _usbDb.IPPrinterSite_Delete_ById(Guid.Parse(id));
                return Json(new { msg = "Delete succeed." });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message });
            }
        }
        #endregion

        // ipprinter

        #region + public async Task<IActionResult> SitePrinterIndex(string siteId)
        public async Task<IActionResult> SitePrinterIndex(string siteId)
        {
            try
            {
                var vm = await _usbDb.IPPrinterSiteVM_Get_BySiteId(Guid.Parse(siteId));
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.GetBaseException().Message;
                return View("Error");
            }
        }
        #endregion

        #region + public async Task<IActionResult> SiteAddPrinter(Tbl_IPPrinterInfo printer)
        public async Task<IActionResult> SiteAddPrinter(Tbl_IPPrinterInfo printer)
        {
            try
            {
                await _usbDb.IPPrinterInfo_Insert(printer);

                return Json(new { Msg = "Printer add succeed." });
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.GetBaseException().Message });
            }
        }
        #endregion

        #region SitePrinterEdit
        public async Task<IActionResult> SitePrinterEdit(int id)
        {
            try
            {
                var printer = await _usbDb.IPPrinterInfo_Get_ById(id);
                return View(printer);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.GetBaseException().Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SitePrinterEdit(Tbl_IPPrinterInfo printer)
        {
            try
            {
                await _usbDb.IPPrinterInfo_Update(printer);
                return Json(new { Msg = "Printer update succeed." });
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.GetBaseException().Message });
            }
        }
        #endregion

        #region + public async Task<IActionResult> SitePrinterDelete(int id)
        [HttpPost]
        public async Task<IActionResult> SitePrinterDelete(int id)
        {
            try
            {
                await _usbDb.IPPrinterInfo_Delete_ById(id);
                return Json(new { Msg = "Printer delete succeed." });
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.GetBaseException().Message });
            }
        }
        #endregion
    }
}
