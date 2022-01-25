using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginUserManager;
using Microsoft.AspNetCore.Authorization;

namespace USBAdminWebMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly LoginUserService _loginService;

        public AccountController(LoginUserService loginService)
        {
            _loginService = loginService;
        }

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


        public async Task<IActionResult> Logout()
        {
            try
            {
                await _loginService.Logout();
                return RedirectToAction("Login","Account");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
