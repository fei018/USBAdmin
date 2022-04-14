using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    [Authorize]
    public class ComputerController : Controller
    {
        private readonly USBDBHelp _usbDb;

        public ComputerController(USBDBHelp usbDbHelp)
        {
            _usbDb = usbDbHelp;
        }

        #region ComputerIndex
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ComputerList(int page, int limit)
        {
            try
            {
                var (totalCount, list) = await _usbDb.ComputerInfo_Get_List(page, limit);
                return JsonResultHelp.LayuiTableData(totalCount, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region UsbLog
        public async Task<IActionResult> UsbLog(int Id)
        {
            try
            {
                var query = await _usbDb.ComputerInfo_Get_ById(Id);
                return View(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UsbLogList(string comIdentity, int page, int limit)
        {
            try
            {
                (int totalCount, List<Tbl_UsbLog> list) = await _usbDb.UsbLog_Get_List_ByComputerIdentity(comIdentity, page, limit);

                return JsonResultHelp.LayuiTableData(totalCount, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region PrintJob
        public async Task<IActionResult> PrintJob(int Id)
        {
            try
            {
                var query = await _usbDb.ComputerInfo_Get_ById(Id);
                return View(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> PrintJobList(string computerIdentity, int page, int limit)
        {
            try
            {
                var (total, list) = await _usbDb.PrintJobLog_Get_List_ByComputerIdentity(computerIdentity, page, limit);
                return JsonResultHelp.LayuiTableData(total, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region Delete()
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usbDb.ComputerInfo_Delete_ById(id);
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
