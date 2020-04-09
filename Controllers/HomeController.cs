﻿/////////////////adding comment to not delete using system when saving
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
using Microsoft.AspNetCore.Hosting;
using System.IO;

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
        private IHostingEnvironment _hostingEnvironment;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context, IHostingEnvironment environment)
        {
            dbContext = context;
            _hostingEnvironment = environment;
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
            List<Request> NearbyRequests = dbContext.Requests.Include(a => a.Creator).Where(a => a.Creator.ZipCode == CurrentUser.ZipCode).ToList();
            return View(NearbyRequests);
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
                    return View("HelpRegView");
                }
                string fileName = null;
                if (HelpUser.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    fileName = HelpUser.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    HelpUser.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                HelpUser.ImageUrl = fileName;
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPw = hasher.HashPassword(HelpUser, HelpUser.Password);
                HelpUser.Password = hashedPw;
                dbContext.Add(HelpUser);
                dbContext.SaveChanges();
                UserSession = HelpUser.UserId;
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
                string fileName = null;
                if (HelperUser.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    fileName = HelperUser.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    HelperUser.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                HelperUser.ImageUrl = fileName;
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
        public IActionResult submitRequest(Request newRequest)
        {
            // if (ModelState.IsValid)
            // {
            User userfromDb = dbContext.Users.FirstOrDefault(a => a.UserId == UserSession);
            newRequest.UserID = userfromDb.UserId;
            dbContext.Add(newRequest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
            // }
            // return View("RequestForm");
        }

        [HttpGet("together/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("LandingPage");
        }

    }
}