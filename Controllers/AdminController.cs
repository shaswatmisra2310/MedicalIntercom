﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalIntercomProject.Models;

namespace MedicalIntercomProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            RedirectToAction("LoggedIn", "Login");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNewUser( NewUserViewModel newuser)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index","Admin");

            var temp = newuser.username;

            return View("Created","Admin");

        }
    }
}