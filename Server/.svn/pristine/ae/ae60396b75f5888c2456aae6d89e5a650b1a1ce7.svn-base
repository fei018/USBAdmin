using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    [Authorize]
    public class USBController : Controller
    {
        private readonly USBAdminDatabaseHelp _usbDb;
        private readonly HttpContext _httpContext;
        private readonly EmailHelp _email;

        public USBController(IHttpContextAccessor httpContextAccessor, USBAdminDatabaseHelp usbDb, EmailHelp emailHelp)
        {
            _httpContext = httpContextAccessor.HttpContext;

            _usbDb = usbDb;          
            _email = emailHelp;
        }


        #region Register
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> RegisterUsb(Tbl_UsbRequest usb)
        {
            try
            {
                await _usbDb.UsbRequest_Insert(usb);
                return JsonResultHelp.Ok("Register Success.");
            }
            catch (Exception ex)
            {
                return JsonResultHelp.Error(ex.Message);
            }
        }
        #endregion

        // UsbHistory

        #region History
        public IActionResult History()
        {
            return View();
        }

        public async Task<IActionResult> HistoryList(int page, int limit)
        {
            try
            {
                var (totalCount, list) = await _usbDb.Get_UsbHistoryVMList(page, limit);
                return JsonResultHelp.LayuiTableData(totalCount, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion


        // UsbRequest

        #region RequestDetail(int id)
        public async Task<IActionResult> RequestDetail(int id)
        {
            try
            {
                var query = await _usbDb.UsbRequestVM_Get_ById(id);
                return View(query);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        #endregion

        #region RequestReview
        public IActionResult RequestReview()
        {
            return View();
        }

        public async Task<IActionResult> RequestReviewList(int page, int limit)
        {
            try
            {
                var (total, list) = await _usbDb.UsbRequestVM_Get_ByStateType(page, limit, UsbRequestStateType.UnderReview);
                return JsonResultHelp.LayuiTableData(total, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region RequestApprove
        public IActionResult RequestApprove()
        {
            return View();
        }

        public async Task<IActionResult> RequestApproveList(int page, int limit)
        {
            try
            {
                var (total, list) = await _usbDb.UsbRequestVM_Get_ByStateType(page, limit, UsbRequestStateType.Approve);
                return JsonResultHelp.LayuiTableData(total, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region RequestReject
        public IActionResult RequestReject()
        {
            return View();
        }

        public async Task<IActionResult> RequestRejectList(int page, int limit)
        {
            try
            {
                var (total, list) = await _usbDb.UsbRequestVM_Get_ByStateType(page, limit, UsbRequestStateType.Reject);
                return JsonResultHelp.LayuiTableData(total, list);
            }
            catch (Exception ex)
            {
                return JsonResultHelp.LayuiTableData(ex.Message);
            }
        }
        #endregion

        #region RequestToApprove(int id)
        [HttpPost]
        public async Task<IActionResult> RequestToApprove(int id)
        {
            try
            {
                var usb = await _usbDb.UsbRequest_ToApprove_ById(id);

                await _email.Send_UsbReuqest_Notify_Result_ToUser(usb);

                //ViewBag.OK = "Approve Succeed: " + usb.ToString();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RequestToReject(int id)
        [HttpPost]
        public async Task<IActionResult> RequestToReject(int id)
        {
            try
            {
                var usb = await _usbDb.UsbRequest_ToReject_ById(id);

                await _email.Send_UsbReuqest_Notify_Result_ToUser(usb);

                //ViewBag.OK = "Reject Succeed: " + usb.ToString();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RequestToDelete(int id)
        [HttpPost]
        public async Task<IActionResult> RequestToDelete(int id)
        {
            try
            {
                await _usbDb.UsbRequest_Delete_ById(id);

                //ViewBag.OK = "Delete Succeed.";

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
