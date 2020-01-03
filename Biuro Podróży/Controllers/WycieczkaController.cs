using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Biuro_Podróży.Controllers
{
    public class WycieczkaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}