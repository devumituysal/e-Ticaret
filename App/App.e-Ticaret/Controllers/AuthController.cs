using App.e_Ticaret.Models.ViewModels;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.e_Ticaret.Controllers
{
    public class AuthController : Controller
    {
        [Route("/register")]
        [HttpGet]
        public IActionResult Register()  // kullanıcı kayıt formunu açar
        {
            return View();
        }
        [Route("/register")]
        [HttpPost]
        public IActionResult Register([FromForm] RegisterViewModel registerModel)  // kullanıcı form doldurarak kayıt olur
        {
            return View();
        }
        [Route("/login")]
        [HttpGet]
        public IActionResult Login() // login olma formunu açar
        {
            return View();
        }
        [Route("/login")]
        [HttpPost]
        public IActionResult Login([FromForm] LoginViewModel loginModel)  // login formu doldurularak login olunur
        {
            return View();
        }
        [Route("/forgot-password")]
        [HttpGet]
        public IActionResult ForgotPassword()  // şifremi unuttum formunu açar
        {
            return View();
        }
        [Route("/forgot-password")]
        [HttpPost]
        public IActionResult ForgotPassword([FromForm] object forgotPasswordMailModel) // şifremi unuttum formu doldurularak parola sıfırlama isteği yapılır
        {
            return View();
        }
        [Route("/renew-password/{verificationCode}")]
        [HttpGet]
        public IActionResult RenewPassword([FromRoute] string verificationCode) // kullanıcı mailindeki şifre yenile linkine tıkladığında çalışır..gerekli doğrulama yapıldıktan sonra şifre yenileme formuna gönderilir
        {
            return View();
        }
        [Route("/renew-password")]
        [HttpPost]
        public IActionResult RenewPassword([FromForm] object changePasswordModel)  // kullanıcı formu doldurarak yeni şifre belirler
        {
            return View();
        }
        [Route("/logout")]
        [HttpGet]
        public IActionResult Logout() // ilgili butona basılarak logout olunur
        {
            return RedirectToAction(nameof(Login));
        }

    }
}
