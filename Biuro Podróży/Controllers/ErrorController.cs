using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biuro_Podróży.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch(statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Błąd 404";
                    break;
            }
            return View("Notfound");
        }
    }
}
