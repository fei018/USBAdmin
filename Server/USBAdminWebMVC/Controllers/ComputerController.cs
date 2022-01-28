﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly USBAdminDatabaseHelp _usbDb;

        public ComputerController(USBAdminDatabaseHelp usbDbHelp)
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
                var (totalCount, list) = await _usbDb.PerComputer_Get_List(page, limit);
                return JsonResultHelp.LayuiTableData(totalCount, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region UsbHistory
        public async Task<IActionResult> UsbHistory(int Id)
        {
            try
            {
                var query = await _usbDb.PerComputer_Get_ById(Id);
                return View(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UsbHistoryList(string comIdentity, int page, int limit)
        {
            try
            {
                (int totalCount, List<Tbl_PerUsbHistory> list) = await _usbDb.Get_UsbHistoryListByComputerIdentity(comIdentity, page, limit);

                return JsonResultHelp.LayuiTableData(totalCount, list);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Delete()
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usbDb.PerComputer_Delete_ById(id);
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
