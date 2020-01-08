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
using Microsoft.AspNetCore.Authorization;
using Biuro_Podróży.ViewModel;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Claims;

namespace Biuro_Podróży.Controllers
{
    public class WycieczkaController : Controller
    {
        private readonly BiuroContext _context;
        private readonly IHostingEnvironment _hostEnv;

        public WycieczkaController(BiuroContext context, IHostingEnvironment host)
        {
            _context = context;
            _hostEnv = host;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var biuroContext = _context.Wycieczka.Include(w => w.Jedzenie).Include(w => w.Zakwaterowanie);
            return View(await biuroContext.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa");
            ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id_wycieczki,Miejsce,Data_start,Data_end,Cena,Opis,Id_jedzenia,Id_zakwaterowania")] Wycieczka wycieczka, IFormFile Image)
        {
            string filename = Path.GetFileName(Image.FileName);
            string extension = Path.GetExtension(filename);
            string[] extensions = new string[] { ".jpg", ".png" };
            if (ModelState.IsValid)
            {
                if (extensions.Contains(extension))
                {
                    using (var ms = new MemoryStream())
                    {
                        Image.CopyTo(ms);
                        wycieczka.Image = ms.ToArray();
                    }
                }
                else
                {
                    ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa", wycieczka.Id_jedzenia);
                    ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa", wycieczka.Id_zakwaterowania);
                    return View();
                }
                _context.Add(wycieczka);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "wycieczka");
            }
            ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa", wycieczka.Id_jedzenia);
            ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa", wycieczka.Id_zakwaterowania);
            return View(wycieczka);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id_wycieczki,Miejsce,Data_start,Data_end,Cena,Opis,Id_jedzenia,Id_zakwaterowania")] Wycieczka wycieczka, IFormFile Image)
        {
            if (id != wycieczka.Id_wycieczki)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string filename = Path.GetFileName(Image.FileName);
                    string extension = Path.GetExtension(filename);
                    string[] extensions = new string[] { ".jpg", ".png" };
                    if (extensions.Contains(extension))
                    {
                        using (var ms = new MemoryStream())
                        {
                            Image.CopyTo(ms);
                            wycieczka.Image = ms.ToArray();
                        }
                    }
                    else
                    {
                        ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa", wycieczka.Id_jedzenia);
                        ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa", wycieczka.Id_zakwaterowania);
                        return View();
                    }
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
                return RedirectToAction("index", "wycieczka");
            }
            ViewData["Id_jedzenia"] = new SelectList(_context.Jedzenie, "Id_jedzenia", "Nazwa", wycieczka.Id_jedzenia);
            ViewData["Id_zakwaterowania"] = new SelectList(_context.Zakwaterowanie, "Id_zakwaterowania", "Nazwa", wycieczka.Id_zakwaterowania);
            return View(wycieczka);
        }
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
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

        [Authorize(Roles = "Admin")]
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
