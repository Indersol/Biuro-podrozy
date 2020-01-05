using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biuro_Podróży.Models;
using Microsoft.EntityFrameworkCore;

namespace Biuro_Podróży.Controllers
{
    public class UserController : Controller
    {
        private readonly BiuroContext _context;
        // GET: Klient
        public UserController(BiuroContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
                return RedirectToAction(nameof(Login));
            else
            {
                var biuroContext = _context.Wycieczka_Klient.Include(u => u.User).Include(w => w.Wycieczka).Where(w => w.User.Id_usera == HttpContext.Session.GetInt32("UID"));
                return View(await biuroContext.ToListAsync());
            }
        }

        public IActionResult Zarejestruj()
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
                return View();
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zarejestruj([Bind("Id_usera,Login,Password,ConfirmPassword,Email,Imie,Nazwisko,Miejscowosc,Telefon,Uprawnienia")] User user)
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
            {
                if (ModelState.IsValid)
                {
                    user.Uprawnienia = Uprawnienia.Klient;
                    _context.Add(user);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        var sqlException = ex.InnerException as System.Data.SqlClient.SqlException;
                        if (sqlException.Number == 2601 || sqlException.Number == 2627)
                        {
                            ViewBag.Message = "Już istnieje taki użytkownik!";
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = "Bład podczas dodawania do bazy!";
                            return View();
                        }
                    }
                    ViewBag.Message = user.Login + "Utworzono nowego użytkownika!";
                }
                return View();
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
                return View();
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
            {
                var klient = _context.User.Where(u => u.Login == user.Login && u.Password == user.Password).FirstOrDefault();
                if (klient != null)
                {
                    HttpContext.Session.SetInt32("UID", klient.Id_usera);
                    HttpContext.Session.SetString("ULogin", klient.Login.ToString());
                    HttpContext.Session.SetInt32("Login", 1);
                    HttpContext.Session.SetString("Tryb", klient.Uprawnienia.ToString());
                    TempData["UID"] = klient.Id_usera.ToString();
                    ViewData["ULogin"] = klient.Login.ToString();
                    ViewData["Tryb"] = klient.Uprawnienia.ToString();
                    ViewData["Login"] = true;
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                    ModelState.AddModelError("", "Podany login lub hasło są nie prawidłowe");
                return View();
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("UID");
            HttpContext.Session.Remove("Tryb");
            HttpContext.Session.Remove("ULogin");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private bool UsersExists(int id)
        {
            return _context.User.Any(e => e.Id_usera == id);
        }


        public async Task<IActionResult> Ustawienia()
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
            {
                return NotFound();
            }
            var klient = await _context.User.FindAsync(HttpContext.Session.GetInt32("UID"));
            if (klient == null)
            {
                return NotFound();
            }
            return View(klient);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ustawienia([Bind("Id_usera,Login,Password,ConfirmPassword,Email,Imie,Nazwisko,Miejscowosc,Telefon,2")] User user)
        {
            if (HttpContext.Session.GetInt32("UID") != user.Id_usera)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id_usera))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        private bool UserExists(int id)
        {
           return _context.User.Any(e => e.Id_usera == id);
        }


    }
  
}