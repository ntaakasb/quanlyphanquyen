using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAuthenticateServices _authenticateServices;
        public HomeController(IAuthenticateServices authenticateServices)
        {
            _authenticateServices = authenticateServices;
        }
        public IActionResult Index()
        {
           //var result =  _authenticateServices.Login(new Base.Model.LoginRequestModel()
           // {
           //     username = "gdv.01",
           //     password = "abc@123",
           //     appCode = "APP_ADMIN"
           // });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
