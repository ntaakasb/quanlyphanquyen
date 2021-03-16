using Base;
using Base.Model;
using Base.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public JsonResult InsertUser(UserRequestModel model)
        {
            TraceModel trace = new TraceModel()
            {
                userId = "gdv.01",
                appCode = "APP_ADMIN",
                clientTransId = new Guid().ToString(),
                clientTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")

            };
            PushModel request = new PushModel()
            {
                trace = trace,
                data = model
            };
            var rs = _userService.CreateNewUser(request);
            return Json("1");
        }

    }
}
