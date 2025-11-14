using Microsoft.AspNetCore.Mvc;

namespace App.e_Ticaret.Controllers
{
    public class ProfileController : Controller
    {
        [Route("/profile")]
        [HttpGet]
        public IActionResult Details() // kullanıcı kendi profil sayfasını görür
        {
            return View();
        }

        [Route("/profile")]
        [HttpPost]
        public IActionResult Edit([FromForm] object editMyProfileModel) // kullanıcı kendi sayfasını günceller
        {
            return RedirectToAction(nameof(Details));
        }

        [Route("/my-orders")]
        [HttpGet]
        public IActionResult MyOrders() // geçmiş siparişleri gösterir
        {
            return View();
        }

        [Route("/my-products")]
        [HttpGet]
        public IActionResult MyProducts() // satıcı ise mevcut ürünlerini gösterir
        {
            return View();
        }
    }
}
