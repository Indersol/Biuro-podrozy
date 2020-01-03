using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biuro_Podróży.Models;
using Microsoft.AspNetCore.Http;

namespace Biuro_Podróży.Controllers
{
    public class WycieczkiController : Controller
    {
        private readonly BiuroContext _context;

        public WycieczkiController(BiuroContext context)
        {
            _context = context;
        }

        // GET: Wycieczki
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                var biuroContext = _context.Wycieczka_Klient.Include(w => w.User).Include(w => w.Wycieczka);
                return View(await biuroContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Wycieczki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var wycieczka_Klient = await _context.Wycieczka_Klient
                    .Include(w => w.User)
                    .Include(w => w.Wycieczka)
                    .FirstOrDefaultAsync(m => m.Id_zamowienia == id);
                if (wycieczka_Klient == null)
                {
                    return NotFound();
                }

                return View(wycieczka_Klient);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Wycieczki/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                ViewData["Id_usera"] = new SelectList(_context.User, "Id_usera", "Email");
                ViewData["Id_wycieczki"] = new SelectList(_context.Wycieczka, "Id_wycieczki", "Miejsce");
                return View();
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Wycieczki/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_zamowienia,Id_usera,Id_wycieczki,Bilety")] Wycieczka_Klient wycieczka_Klient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wycieczka_Klient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_usera"] = new SelectList(_context.User, "Id_usera", "Email", wycieczka_Klient.Id_usera);
            ViewData["Id_wycieczki"] = new SelectList(_context.Wycieczka, "Id_wycieczki", "Miejsce", wycieczka_Klient.Id_wycieczki);
            return View(wycieczka_Klient);
        }

        // GET: Wycieczki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var wycieczka_Klient = await _context.Wycieczka_Klient.FindAsync(id);
                if (wycieczka_Klient == null)
                {
                    return NotFound();
                }
                ViewData["Id_usera"] = new SelectList(_context.User, "Id_usera", "Email", wycieczka_Klient.Id_usera);
                ViewData["Id_wycieczki"] = new SelectList(_context.Wycieczka, "Id_wycieczki", "Miejsce", wycieczka_Klient.Id_wycieczki);
                return View(wycieczka_Klient);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Wycieczki/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_zamowienia,Id_usera,Id_wycieczki,Bilety")] Wycieczka_Klient wycieczka_Klient)
        {
            if (id != wycieczka_Klient.Id_zamowienia)
            {
                return NotFound();
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
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_usera"] = new SelectList(_context.User, "Id_usera", "Email", wycieczka_Klient.Id_usera);
            ViewData["Id_wycieczki"] = new SelectList(_context.Wycieczka, "Id_wycieczki", "Miejsce", wycieczka_Klient.Id_wycieczki);
            return View(wycieczka_Klient);
        }

        // GET: Wycieczki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var wycieczka_Klient = await _context.Wycieczka_Klient
                    .Include(w => w.User)
                    .Include(w => w.Wycieczka)
                    .FirstOrDefaultAsync(m => m.Id_zamowienia == id);
                if (wycieczka_Klient == null)
                {
                    return NotFound();
                }

                return View(wycieczka_Klient);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Wycieczki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wycieczka_Klient = await _context.Wycieczka_Klient.FindAsync(id);
            _context.Wycieczka_Klient.Remove(wycieczka_Klient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Wycieczka_KlientExists(int id)
        {
            return _context.Wycieczka_Klient.Any(e => e.Id_zamowienia == id);
        }
    }
}
