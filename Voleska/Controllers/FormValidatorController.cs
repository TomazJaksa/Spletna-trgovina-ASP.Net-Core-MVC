﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Voleska.Areas.Identity.Data;

namespace Voleska.Controllers
{
    
    public class FormValidatorController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public FormValidatorController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse([Bind(Prefix = "Input.Email")] string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result == null)
            {
                ViewData["Invalid"] = false;
                return Json(true);
            }
            else
            {
                ViewData["Invalid"] = false;
                return Json(false);
            }

        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUsernameInUse([Bind(Prefix = "Input.Username")] string username)
        {
            var result = await _userManager.FindByNameAsync(username);
            if (result==null)
            {
                return Json(true);
            }
            else
            {
                ViewData["Invalid"] = false;
                return Json(false);
            }

        }

    }
}
