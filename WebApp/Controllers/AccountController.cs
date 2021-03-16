using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Base.Common;
using Base.Constants;
using Base.Model;
using Base.Model.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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


        public AccountController(IAuthenticateServices authenticateServices)
        {
            _authenticateServices = authenticateServices;

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

            var requestAccessToken = _authenticateServices.Login(new Base.Model.LoginRequestModel()
            {
                username = model.UserAccount,
                password = model.UserSecret,
                appCode = "APP_ADMIN"
            });
            var result = await LoginLogic(requestAccessToken, model);
            if (result)
            {
                return RedirectToLocal(model.ReturnUrl);
            }

            ModelState.AddModelError(string.Empty, requestAccessToken.errorMsg);

            return View(model);
        }

        private async Task<bool> LoginLogic(LoginResultModel requestAccessToken, LoginViewModel model)
        {
            if (requestAccessToken.errorCode == 0)
            {
                var hand = new JwtSecurityTokenHandler();
                var tokenString = requestAccessToken.jwttoken;
                var tokenInfo = hand.ReadJwtToken(tokenString);

                string userId = tokenInfo.Claims.FirstOrDefault().Value;
                var userInfor = new LoggedUserDetailResponseModel();
                //var result = await _identityService.GetCurrentUserDetailAsync(tokenString);
                //if (result.Metadata.Success)
                //{
                //    userInfor = result.Results[0];
                //    if (userInfor.Avatar == null)
                //    {
                //        userInfor.Avatar = string.Empty;
                //    }
                //}

                ClaimsIdentity identityClaim = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identityClaim.AddClaims(
                new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, model.UserAccount ?? tokenInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Actor, GetClaimFromLoginResponseModel(requestAccessToken)),
                    new Claim(ApplicationKey.Token, JsonConvert.SerializeObject(requestAccessToken.jwttoken)),
                    new Claim(ApplicationKey.UserInfo, JsonConvert.SerializeObject(userInfor))
                });
                ClaimsPrincipal principal = new ClaimsPrincipal(identityClaim);

                await HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 principal,
                 new AuthenticationProperties
                 {
                     IsPersistent = false
                 });

                return true;
            }

            return false;
        }
        private string GetClaimFromLoginResponseModel(LoginResultModel result, string claimTypes = ClaimTypes.NameIdentifier)
        {
            var model = result;
            if (model == null || string.IsNullOrEmpty(model.jwttoken))
            {
                return null;
            }

            return GetClaimFromToken(model.jwttoken, claimTypes);
        }

        private string GetClaimFromToken(string token, string claimTypes = ClaimTypes.NameIdentifier)
        {
            var jwtToken = new JwtSecurityToken(token);
            var userIdClaim = jwtToken.Claims.First();

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return null;
            }

            return userIdClaim.Value;
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var appToken = GetTokenFromClaim();
                //if (appToken != null)
                //{
                //    if ((appToken.ExpiresIn - DateTime.UtcNow).TotalMinutes > 0 && !string.IsNullOrEmpty(appToken.Token))
                //    {
                //        _identityService.Logout(appToken.Token);
                //    }
                //}
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Login", "Account");
        }

        private AppToken GetTokenFromClaim()
        {
            var userIdentity = (ClaimsIdentity)User.Identity;
            var apiClaim = userIdentity.FindFirst(ApplicationKey.Token);
            if (apiClaim != null)
            {
                var appToken = JsonConvert.DeserializeObject<AppToken>(apiClaim.Value);
                return appToken;
            }

            return null;
        }
    }
}  