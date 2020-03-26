using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GSAPP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace GSAPP.Controllers
{
    public class HomeController : Controller
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }

        public IActionResult LandingPage()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Form()
        {
            return View();
        }

    }
}
