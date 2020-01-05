using Biuro_Podróży.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signinManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }

        [HttpGet]
        public IActionResult Rejestracja()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Rejestracja(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signinManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

    }
}

