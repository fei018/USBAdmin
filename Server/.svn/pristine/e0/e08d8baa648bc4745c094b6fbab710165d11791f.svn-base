using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using USBModel;
using USBCommon;
using USBAdminWebMVC;

namespace USBAdminWebMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly USBAdminDatabaseHelp _usbDb;

        public HomeController(USBAdminDatabaseHelp usbDb)
        {
            _usbDb = usbDb;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InitMenu()
        {
            var json = USBAdminHelp.GetInitMenuJson();
            return Content(json, MimeTypeMap.GetMimeType("json"));
        }

        public async Task<IActionResult> Welcome()
        {
            try
            {
                var welcomeVM = new WelcomeVM();
                welcomeVM.UsbRequestApproveCount = await _usbDb.UsbRequest_TotalCount_ByState(UsbRequestStateType.Approve);
                welcomeVM.UsbRequestRejectCount = await _usbDb.UsbRequest_TotalCount_ByState(UsbRequestStateType.Reject);
                welcomeVM.UsbRequestUnderReviewCount = await _usbDb.UsbRequest_TotalCount_ByState(UsbRequestStateType.UnderReview);
                welcomeVM.ComputerCount = await _usbDb.PerComputer_Get_TotalCount();

                return View(welcomeVM);
            }
            catch (Exception ex)
            {
                return View("Welcome", ex.Message);
            }
        }
    }
}
