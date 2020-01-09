using Biuro_Podróży.Models;
using Biuro_Podróży.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Biuro_Podróży.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
            this.roleManager = roleManager;
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
                var role = await roleManager.FindByNameAsync("Klient");

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Nie znaleziono uprawnienia";
                    return View("NotFound");
                }
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
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
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =(await signinManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signinManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signinManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if(remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Błąd: {remoteError}");
                return View("Login", loginViewModel);
            }
            var info = await signinManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                ModelState.AddModelError(string.Empty, "Błąd logowania");
                return View("Login", loginViewModel);
            }
            var signInResult = await signinManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if(signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if(email!= null)
                {
                    var user = await userManager.FindByEmailAsync(email);
                    var role = await roleManager.FindByNameAsync("Klient");
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await userManager.CreateAsync(user);
                        await userManager.AddToRoleAsync(user, role.Name);
                    }
                    await userManager.AddLoginAsync(user, info);
                    await signinManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                ViewBag.ErrorTitle = $"Nie można pobrać maila od {info.LoginProvider}";
                ViewBag.ErrorMessage = $"Skontaktuj sie z administratorem systemu";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
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
            return RedirectToAction("BadPass");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BadPass()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EdycjaKonta()
        {
            ClaimsPrincipal currentUser = this.User;
            var user = await userManager.FindByNameAsync(currentUser.Identity.Name);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono użytkownika";
                return View("NotFound");
            }
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                Miasto = user.Miasto,
                NrTel = user.NrTel,
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EdycjaKonta(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono użytkownika";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.Imie = model.Imie;
                user.Nazwisko = model.Nazwisko;
                user.Miasto = model.Miasto;
                user.NrTel = model.NrTel;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }



    }
}

