using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Repositories.Interfaces;
using App.e_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.e_Ticaret.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IDataRepository<UserEntity> _repo;

        public AuthController(IDataRepository<UserEntity> repo)
        {
            _repo=repo;   
        }

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()  // kullanıcı kayıt formunu açar
        {
            return View();
        }
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterViewModel registerModel)  // kullanıcı form doldurarak kayıt olur
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            var user = new UserEntity
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                Password = registerModel.Password,
                RoleId = 3,
                CreatedAt = DateTime.UtcNow,
            };

            await _repo.AddAsync(user);

            ViewBag.SuccessMessage = "Kayıt işlemi başarılı. Giriş yapabilirsiniz.";
            ModelState.Clear();

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
        public async Task<IActionResult> LoginAsync([FromForm] LoginViewModel loginModel)  // login formu doldurularak login olunur
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var user = await _repo.GetAll()
                .Include(u=>u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }

            await LogInAsync(user);

            return RedirectToAction("Index", "Home");
        }

        private async Task LogInAsync(UserEntity user)
        {
            if (user == null)
            {
                return;
            }

            SetCookie("userId", user.Id.ToString());
            SetCookie("mail", user.Email);
            SetCookie("name", user.FirstName);
            SetCookie("surname", user.LastName);
            SetCookie("role", user.RoleId.ToString());

            await Task.CompletedTask;
        }

        [Route("/forgot-password")]
        [HttpGet]
        public IActionResult ForgotPassword()  // şifremi unuttum formunu açar
        {
            return View();
        }
        [Route("/forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync([FromForm] ForgotPasswordViewModel forgotPasswordMailModel) // şifremi unuttum formu doldurularak parola sıfırlama isteği yapılır
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordMailModel);
            }

            var user = await _repo.GetAll().FirstOrDefaultAsync(u => u.Email == forgotPasswordMailModel.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(forgotPasswordMailModel);
            }

            // Şifre sıfırlama kodu oluşturulacak ve kullanıcıya mail gönderilecek...
            await SendResetPasswordEmailAsync(user);

            ViewBag.SuccessMessage = "Şifre sıfırlama maili gönderildi. Lütfen e-posta adresinizi kontrol edin.";
            ModelState.Clear();

            return View();
        }

        private async Task SendResetPasswordEmailAsync(UserEntity user)
        {
            // Şifre sıfırlama maili gönderme kodları...
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...
            if (user == null)
            {
                return;
            }

            await Task.CompletedTask;
        }

        [Route("/renew-password/{verificationCode}")]
        [HttpGet]
        public IActionResult RenewPassword([FromRoute] string verificationCode) // kullanıcı mailindeki şifre yenile linkine tıkladığında çalışır..gerekli doğrulama yapıldıktan sonra şifre yenileme formuna gönderilir
        {
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...
            return View();
        }
        [Route("/renew-password")]
        [HttpPost]
        public IActionResult RenewPassword([FromForm] object changePasswordModel)  // kullanıcı formu doldurarak yeni şifre belirler
        {
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...
            return View();
        }
        [Route("/logout")]
        [HttpGet]
        public async Task<IActionResult> LogoutAsync() // ilgili butona basılarak logout olunur
        {

            await LogoutUser();
            return RedirectToAction(nameof(Login));
        }
        private async Task LogoutUser()
        {
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...

            RemoveCookie("userId");
            RemoveCookie("mail");
            RemoveCookie("name");
            RemoveCookie("surname");
            RemoveCookie("role");
        }
    }
}
