using Base.Common;
using Base.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Services.Token
{
    public class TokenBaseService : ITokenBaseService
    {
        //private readonly IAuthenticationRestClient _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<ApplicationSetting> _applicationSettings;
        public TokenBaseService(
            //IAuthenticationRestClient identityService,
            IHttpContextAccessor httpContextAccessor, IOptions<ApplicationSetting> applicationSettings)
        {
            //_identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _applicationSettings = applicationSettings;
        }

        public string GetToken()
        {
            var token = string.Empty;
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
            {
                return token;
            }

            var userIdentity = (ClaimsIdentity)user.Identity;
            var apiClaim = userIdentity.FindFirst(ApplicationKey.Token);
            if (apiClaim != null)
            {
                var appToken = JsonConvert.DeserializeObject<AppToken>(apiClaim.Value);
                token = appToken.Token;

                var expired = (appToken.ExpiresIn - DateTime.UtcNow).TotalMinutes < 10;

                // Check token will expired in 10 minutes to refresh token
                if (!string.IsNullOrEmpty(token) && expired)
                {
                    //var request = new RefreshTokenRequestModel()
                    //{
                    //    RefreshToken = appToken.RefreshToken,
                    //    AccessToken = appToken.Token
                    //};

                    //var newToken = _identityService.RefreshToken(request);

                    //if (newToken.Success)
                    //{
                    //    userIdentity.RemoveClaim(apiClaim);
                    //    userIdentity.AddClaim(new Claim(ApplicationKey.Token, JsonConvert.SerializeObject(newToken.First())));
                    //    token = newToken.First().Token;

                    //    ClaimsIdentity identityClaim = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    //    identityClaim.AddClaims(
                    //        new List<Claim>()
                    //        {
                    //            new Claim(ClaimTypes.Name, userIdentity.FindFirst(ClaimTypes.Name).Value),
                    //            new Claim(ClaimTypes.NameIdentifier, userIdentity.FindFirst(ClaimTypes.NameIdentifier).Value),
                    //            new Claim(ApplicationKey.Token, JsonConvert.SerializeObject(newToken.First())),
                    //            new Claim(ApplicationKey.UserInfo, userIdentity.FindFirst(ApplicationKey.UserInfo).Value)
                    //        });

                    //    _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    //    ClaimsPrincipal principal = new ClaimsPrincipal(identityClaim);

                    //    _httpContextAccessor.HttpContext.SignInAsync(
                    //        CookieAuthenticationDefaults.AuthenticationScheme,
                    //        principal,
                    //        new AuthenticationProperties()
                    //        {
                    //            IsPersistent = false
                    //        });
                    //}
                }
            }

            return token;
        }
    }
}
