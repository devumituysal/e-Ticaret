using App.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class AuthController : Controller
    {
        [Route("/login")]
        [HttpGet]
        public IActionResult Login()      //login butonu ile admin login formunu açar
        {
            return View();
        }

        [Route("/login")]                 //admin login formu doldurularak admin girişi yapılır
        [HttpPost]
        public IActionResult Login([FromForm] LoginViewModel loginModel)
        {
            return View();
        }

        [Route("/logout")]
        [HttpGet]
        public IActionResult Logout()         //logout butonuna basılınca admin logout olur
        {
            // logout kodları...

            return RedirectToAction(nameof(Login));
        }
    }
}
