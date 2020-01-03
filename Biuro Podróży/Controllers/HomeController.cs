using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Biuro_Podróży.Models;
using ReflectionIT.Mvc.Paging;
using Microsoft.EntityFrameworkCore;

namespace Biuro_Podróży.Controllers
{
    public class HomeController : Controller
    {
        private readonly BiuroContext _context;
        public HomeController(BiuroContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int strona = 1)
        {
            var biuroContext = _context.Wycieczka.Include(w => w.Jedzenie).Include(w => w.Zakwaterowanie);
            return View(await biuroContext.ToListAsync());
            //return View();
        }


        public IActionResult Privacy()
        {
            ViewData["Message"] = "Polityka prywatnośći";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
