using Microsoft.AspNetCore.Mvc;

namespace App.e_Ticaret.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()  //  anasayfa
        {
            return View();
        }
        [Route("/about-us")]
        [HttpGet]
        public IActionResult AboutUs()   // hakkýmýzda
        {
            return View();
        }
        [Route("/contact")]
        [HttpGet]
        public IActionResult Contact() // iletiþim formunu açar
        {
            return View();
        }
        [Route("/contact")]
        [HttpPost]
        public IActionResult Contact([FromForm] object newContactMessageModel) // iletiþim formu doldurulduktan sonra gönderilme iþini yapar
        {
            return View();
        }
        [Route("/product/list")]
        [HttpGet]
        public IActionResult Listing()  // ürünler sayfasýný açar
        {
            return View();
        }
        [Route("/product/{productId:int}/details")]
        [HttpGet]
        public IActionResult ProductDetail([FromRoute] int productId) // bir ürünün üzerindeki buton ile çalýþýr ve detaylarýný gösterir
        {
            return View();
        }
    }
}
