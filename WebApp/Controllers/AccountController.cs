using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Base.Common;
using Base.Model.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using WebApp.Models;

namespace WebApp.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAuthenticateServices _authenticateServices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(IAuthenticateServices authenticateServices, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _authenticateServices = authenticateServices;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
            };

            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl = "/Home/Index")
        {
            if (Url.IsLocalUrl(returnUrl) && returnUrl.ToLower().IndexOf("logout") < 0)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        public JsonResult LoginSubmit(string userName, string passWord)
        {
            var result = _authenticateServices.Login(new Base.Model.LoginRequestModel()
            {
                username = userName,
                password = passWord,
                appCode = "APP_ADMIN"
            });

            if(result.errorCode == 0)
            {
                return Json(1);
            }
            return Json(0);
            // return await _chartService.GetChartOverview(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _authenticateServices.Login(new Base.Model.LoginRequestModel()
            {
                username = model.UserAccount,
                password = model.UserSecret,
                appCode = "APP_ADMIN"
            });


            return View(model);
        }

    }
}  