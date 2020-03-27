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

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("Detail")]
        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult Form()
        {
            return View();
        }

        [HttpPost("registeruser")]
        public IActionResult RegisterUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(i => i.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists!");
                    return View("Index");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPw = hasher.HashPassword(newUser, newUser.Password);
                newUser.Password = hashedPw;
                dbContext.Add(newUser);
                dbContext.SaveChanges();
                UserSession = newUser.UserId;
                if (newUser.Status == "No")
                {
                    return RedirectToAction("Dashboard");
                }
                return RedirectToAction("Form");
            }
            return View("Register");
        }

        [HttpPost("loginuser")]
        public IActionResult LoginUser(Login currentUser)
        {
            if (ModelState.IsValid)
            {
                var UserInDb = dbContext.Users.FirstOrDefault(i => i.Email == currentUser.LoginEmail);
                if (UserInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Credentials");
                    return View("Index");
                }
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(currentUser, UserInDb.Password, currentUser.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Credentials");
                    return View("Index");
                }
                UserSession = UserInDb.UserId;
                return RedirectToAction("Dashboard");
            }
            return View("Login");
        }

        [HttpPost("requesthelp")]
        public IActionResult RequestHelp(Request newRequest)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newRequest);
                dbContext.SaveChanges();
                return RedirectToAction("Detail");
            }
            return View("RequestForm");
        }

    }
}
