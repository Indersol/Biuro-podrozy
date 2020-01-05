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
    public class AdminController : Controller
    {
        private readonly BiuroContext _context;

        public AdminController(BiuroContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Tryb") == "Admin")
            {
                var biuroContext = _context.User.Where(w => w.Uprawnienia != Uprawnienia.Admin);
                return View(await biuroContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.User
                    .FirstOrDefaultAsync(m => m.Id_usera == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Tryb") == "Admin")
            {
                return View();
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_usera,Login,Password,Email,Imie,Nazwisko,Miejscowosc,Telefon,Uprawnienia")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Uprawnienia = Uprawnienia.Moderator;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.User.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_usera,Login,Password,Email,Imie,Nazwisko,Miejscowosc,Telefon,Uprawnienia")] User user)
        {
            if (id != user.Id_usera)
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

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Tryb") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.User
                    .FirstOrDefaultAsync(m => m.Id_usera == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id_usera == id);
        }
    }
}
