using System.Diagnostics;
using App.e_Ticaret.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.e_Ticaret.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        [Route("/products/list")]
        public IActionResult Listing()
        {
            return View();
        }

    }
}
