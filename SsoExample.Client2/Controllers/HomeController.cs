using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microex.All.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SsoExample.Client2.Models;

namespace SsoExample.Client2.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        public IActionResult Index()
        {
            ViewBag.UserJson = HttpContext?.User?.Identity?.ToJson();
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            return SignOut(OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FrontChannelLogout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> SilentLogin()
        {
            return Ok(HttpContext?.User?.Identity?.ToJson());
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
    }
}
