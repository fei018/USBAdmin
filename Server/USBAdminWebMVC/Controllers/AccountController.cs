using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginUserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace USBAdminWebMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly LoginUserService _loginService;

        public HttpContext _httpConext { get; }

        public AccountController(LoginUserService loginService, IHttpContextAccessor httpContextAccessor)
        {
            _loginService = loginService;

            _httpConext = httpContextAccessor.HttpContext;
        }

        #region Login

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var query = await _loginService.Login(username, password);
            if (query.IsSucceed)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = query.Message;
                return View();
            }
        }
        #endregion

        #region Logout()
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _loginService.Logout();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Setting
        public async Task<IActionResult> Setting()
        {
            try
            {
                var result = await _loginService.GetUserByName(_httpConext.User.Identity.Name);
                if (result.IsSucceed)
                {
                    return View(result.LoginUser);
                }
                else
                {
                    ViewBag.Error = result.Message;
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Setting(LoginUser user)
        {
            try
            {
                var result = await _loginService.UpdateLoginUser(user);
                if (result.IsSucceed)
                {
                    return Json(new { msg = "Update succeed." });
                }
                else
                {
                    return Json(new { msg = result.Message });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
