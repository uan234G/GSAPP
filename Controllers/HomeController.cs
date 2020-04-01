﻿﻿﻿using System;
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

        [HttpGet("together/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("together/select-role")]
        public IActionResult SelectRole()
        {
            return View();
        }
        [HttpGet("together/helper")]
        public IActionResult HelperRegView()
        {
            return View();
        }
        [HttpGet("together/needofhelp")]
        public IActionResult HelpRegView()
        {
            return View();
        }

        [HttpGet("together/dashboard")]
        public IActionResult Dashboard()
        {
            if (UserSession == null)
            {
                return RedirectToAction("LandingPage");
            }
            User CurrentUser = dbContext.Users.FirstOrDefault(a => a.UserId == UserSession);
            List<User> NearbyUsers = dbContext.Users.Include(a => a.RequestsCreated).Where(a => a.ZipCode == CurrentUser.ZipCode).ToList();
            return View(NearbyUsers);
        }
        [HttpGet("View/{Uid}/Details")]
        public IActionResult Detail(int Uid)
        {
            if (UserSession == null)
            {
                return RedirectToAction("LandingPage");
            }
            User DetailsFor = dbContext.Users.FirstOrDefault(q => q.UserId == Uid);
            return View(DetailsFor);
        }

        public IActionResult RequestForm()
        {
            if (UserSession == null)
            {
                return RedirectToAction("LandingPage");
            }
            return View();
        }



        [HttpPost("together/register/help-user")]
        public IActionResult HelpReg(User HelpUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(i => i.Email == HelpUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists!");
                    return View("HelpReg");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPw = hasher.HashPassword(HelpUser, HelpUser.Password);
                HelpUser.Password = hashedPw;
                dbContext.Add(HelpUser);
                dbContext.SaveChanges();
                UserSession = HelpUser.UserId;
                // if (HelpUser.Status == false)
                // {
                //     return RedirectToAction("Dashboard");
                // }
                return RedirectToAction("RequestForm");
            }
            return View("HelpRegView");
        }

        [HttpPost("together/register/helper-user")]
        public IActionResult HelperReg(User HelperUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(i => i.Email == HelperUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists!");
                    return View("HelperReg");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPw = hasher.HashPassword(HelperUser, HelperUser.Password);
                HelperUser.Password = hashedPw;
                dbContext.Add(HelperUser);
                dbContext.SaveChanges();
                UserSession = HelperUser.UserId;

                return RedirectToAction("Dashboard");
            }
            return View("HelperRegView");
        }

        [HttpPost("together/login/user")]
        public IActionResult LoginUser(Login currentUser)
        {
            if (ModelState.IsValid)
            {
                var UserInDb = dbContext.Users.FirstOrDefault(i => i.Email == currentUser.LoginEmail);
                if (UserInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                    return View("Login");
                }
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(currentUser, UserInDb.Password, currentUser.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                    return View("Login");
                }
                UserSession = UserInDb.UserId;
                return RedirectToAction("Dashboard");
            }
            return View("Login");
        }

        [HttpPost("together/request-help")]
        public IActionResult RequestHelp(Request newRequest)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newRequest);
                dbContext.SaveChanges();
                newRequest.UserID = (int)UserSession;
                return RedirectToAction("Dashboard");
            }
            return View("RequestForm");
        }

        [HttpGet("together/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("LandingPage");
        }

    }
}