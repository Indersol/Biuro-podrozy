using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biuro_Podróży.Models;
using Biuro_Podróży.Controllers;
using Microsoft.AspNetCore.Http;

namespace Biuro_Podróży.Controllers
{
    public class ModeratorController : Controller
    {
        private readonly BiuroContext _context;

        public ModeratorController(BiuroContext context)
        {
            _context = context;
        }

        // GET: Moderator
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                var biuroContext = _context.Wycieczka.Include(w => w.Jedzenie).Include(w => w.Zakwaterowanie);
                return View(await biuroContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Moderator/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var wycieczka = await _context.Wycieczka
                    .Include(w => w.Jedzenie)
                    .Include(w => w.Zakwaterowanie)
                    .FirstOrDefaultAsync(m => m.Id_wycieczki == id);
                if (wycieczka == null)
                {
                    return NotFound();
                }

                return View(wycieczka);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Moderator/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa");
                ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa");
                return View();
            }
            return RedirectToAction("Index", "Home", new { area = "" });

        }

        // POST: Moderator/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_wycieczki,Miejsce,Data_start,Data_end,Cena,Opis,Id_jedzenia,Id_zakwaterowania")] Wycieczka wycieczka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wycieczka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa", wycieczka.Id_jedzenia);
            ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa", wycieczka.Id_zakwaterowania);
            return View(wycieczka);
        }

        // GET: Moderator/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var wycieczka = await _context.Wycieczka.FindAsync(id);
                if (wycieczka == null)
                {
                    return NotFound();
                }
                ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa", wycieczka.Id_jedzenia);
                ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa", wycieczka.Id_zakwaterowania);
                return View(wycieczka);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Moderator/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_wycieczki,Miejsce,Data_start,Data_end,Cena,Opis,Id_jedzenia,Id_zakwaterowania")] Wycieczka wycieczka)
        {
            if (id != wycieczka.Id_wycieczki)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wycieczka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WycieczkaExists(wycieczka.Id_wycieczki))
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
            ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa", wycieczka.Id_jedzenia);
            ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa", wycieczka.Id_zakwaterowania);
            return View(wycieczka);
        }

        // GET: Moderator/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Moderator" || HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var wycieczka = await _context.Wycieczka
                    .Include(w => w.Jedzenie)
                    .Include(w => w.Zakwaterowanie)
                    .FirstOrDefaultAsync(m => m.Id_wycieczki == id);
                if (wycieczka == null)
                {
                    return NotFound();
                }

                return View(wycieczka);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Moderator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wycieczka = await _context.Wycieczka.FindAsync(id);
            _context.Wycieczka.Remove(wycieczka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WycieczkaExists(int id)
        {
            return _context.Wycieczka.Any(e => e.Id_wycieczki == id);
        }
    }
}
