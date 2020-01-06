using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Biuro_Podróży.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biuro_Podróży.Controllers
{
    [AllowAnonymous]
    public class _WycieczkaController : Controller
    {
        private readonly BiuroContext _context;
        public _WycieczkaController(BiuroContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
             var biuroContext = _context.Wycieczka.Include(w => w.Jedzenie).Include(w => w.Zakwaterowanie);
             return View(await biuroContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wycieczka = await _context.Wycieczka
                .Include(w => w.Jedzenie).Include(w => w.Zakwaterowanie)
                .FirstOrDefaultAsync(m => m.Id_wycieczki == id);
            if (wycieczka == null)
            {
                return NotFound();
            }

            return View(wycieczka);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("Id_zamowienia,Id_usera,Id_wycieczki,Bilety")] Wycieczka_Klient wycieczka_Klient)
        {

            if (ModelState.IsValid)
            {
                //wyciąganie id z IdentityUser czy raczej ApplicationUser po zmianie
                ClaimsPrincipal currentUser = this.User;
                wycieczka_Klient.Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                wycieczka_Klient.Id_wycieczki = id;
                _context.Add(wycieczka_Klient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Id_wycieczki"] = new SelectList(_context.Wycieczka, "Id_wycieczki", "Miejsce", wycieczka_Klient.Id_wycieczki);
            ViewBag.Message = " Dodany to agregatora!";
            return View();
        }
    }
}