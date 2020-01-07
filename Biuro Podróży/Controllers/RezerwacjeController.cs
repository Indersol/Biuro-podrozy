using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Biuro_Podróży.Models;
using Biuro_Podróży.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biuro_Podróży.Controllers
{
    public class RezerwacjeController : Controller
    {
        private readonly BiuroContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        public RezerwacjeController(BiuroContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Moje()
        {
            ClaimsPrincipal currentUser = this.User;
            var biuroContext = _context.Wycieczka_Klient.Include(u => u.ApplicationUser).Include(w => w.Wycieczka)
                .Where(w => w.ApplicationUser.Id == currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View(await biuroContext.ToListAsync());
        }
        public async Task<IActionResult> Wszystkie()
        {
            var biuroContext = _context.Wycieczka_Klient.Include(w => w.Wycieczka).Include(k => k.ApplicationUser);
            return View(await biuroContext.ToListAsync());
        }

        [Authorize]
        public IActionResult RKlient(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono użytkownika";
                return View("NotFound");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RKlient(int id, [Bind("Id_zamowienia,Id_usera,Id_wycieczki,Bilety")] Wycieczka_Klient wycieczka_Klient)
        {

            if (ModelState.IsValid)
            {
                //wyciąganie id z IdentityUser czy raczej ApplicationUser po zmianie
                ClaimsPrincipal currentUser = this.User;
                wycieczka_Klient.Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                wycieczka_Klient.Id_wycieczki = id;
                _context.Add(wycieczka_Klient);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "Wycieczka");
            }

            ViewData["Id_wycieczki"] = id;
            ViewBag.Message = " Dodany to agregatora!";
            return View(wycieczka_Klient);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? idz)
        {
            if (idz == null)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono użytkownika";
                return View("NotFound");
            }
            var wycieczka_Klient = await _context.Wycieczka_Klient.FindAsync(idz);
            var user = await userManager.FindByIdAsync(wycieczka_Klient.Id);
            if (wycieczka_Klient == null)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono użytkownika";
                return View("NotFound");
            }
            ViewData["Id_usera"] = user.Id;
            ViewBag.User = user.UserName;
            ViewData["Id_wycieczki"] = new SelectList(_context.Wycieczka, "Id_wycieczki", "Miejsce", wycieczka_Klient.Id_wycieczki);
            return View(wycieczka_Klient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([Bind("Id_zamowienia,Id,Id_wycieczki,Bilety")] Wycieczka_Klient wycieczka_Klient)
        {

            if (wycieczka_Klient.Id_zamowienia == 0)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono rezerwacji";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wycieczka_Klient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Wycieczka_KlientExists(wycieczka_Klient.Id_zamowienia))
                    {
                        ViewBag.ErrorMessage = $"Nie znaleziono rezerwacji";
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Wszystkie", "Rezerwacje");
            }
            var user = await userManager.FindByIdAsync(wycieczka_Klient.Id);
            ViewData["Id_usera"] = user.Id;
            ViewBag.User = user.UserName;
            ViewData["Id_wycieczki"] = new SelectList(_context.Wycieczka, "Id_wycieczki", "Miejsce", wycieczka_Klient.Id_wycieczki);
            return View(wycieczka_Klient);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? idz)
        {
            if (idz == null)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono rezerwacji";
                return View("NotFound");
            }

            var wycieczka_Klient = await _context.Wycieczka_Klient
                .Include(k => k.ApplicationUser)
                .Include(w => w.Wycieczka)
                .FirstOrDefaultAsync(m => m.Id_zamowienia == idz);
            if (wycieczka_Klient == null)
            {
                ViewBag.ErrorMessage = $"Nie znaleziono rezerwacji";
                return View("NotFound");
            }
            return View(wycieczka_Klient);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id_zamowienia,Id,Id_wycieczki,Bilety")] Wycieczka_Klient w)
        {
            var wycieczka_Klient = await _context.Wycieczka_Klient.FindAsync(w.Id_zamowienia);
            _context.Wycieczka_Klient.Remove(wycieczka_Klient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Wszystkie", "Rezerwacje");
        }

        private bool Wycieczka_KlientExists(int idz)
        {
            return _context.Wycieczka_Klient.Any(e => e.Id_zamowienia == idz);
        }
    }
}
