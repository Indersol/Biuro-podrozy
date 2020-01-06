using Biuro_Podróży.Models;
using Biuro_Podróży.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Wyloguj()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public IActionResult Rejestracja()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            //Jquerry sprawdzające czy podany email nie został już wcześniej wykorzystany
            //element JS
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Podany email {email} jest już używany");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Rejestracja(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
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

        [HttpGet]
        public IActionResult Logowanie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logowanie(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signinManager.PasswordSignInAsync(model.Email, model.Password, model.ReamemberMe, false);

                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Błąd logowania");
            }
            return View(model);
        }

    }
}

