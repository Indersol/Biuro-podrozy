using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Biuro_Podróży.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biuro_Podróży.Controllers
{
    public class RezerwacjeController : Controller
    {
        private readonly BiuroContext _context;
        public RezerwacjeController(BiuroContext context)
        {
            _context = context;
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
                return NotFound();
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
    }
}
