using App.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace App.e_Ticaret.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [Route("/product")]
        [HttpGet]
        public IActionResult Create() // yeni bir ürün ekleme formunu açar ( muhtemel senaryo sadece satıcılar için)
        {
            return View();
        }

        [Route("/product")]
        [HttpPost]
        public IActionResult Create([FromForm] object newProductModel) // form doldurularak yeni ürün ekleme işini yapar
        {
            return View();
        }

        [Route("/product/{productId:int}/edit")]
        [HttpGet]
        public IActionResult Edit([FromRoute] int productId) // mevcut ürün üzerindeki buton ile edit sayfasını açar ( yada ürün detayları)
        {
            return View();
        }

        [Route("/product/{productId:int}/edit")]
        [HttpPost]
        public IActionResult Edit([FromRoute] int productId, [FromForm] object editProductModel) // form doldurularak ürün detayları güncellenir
        {
            return View();
        }

        [Route("/product/{productId:int}/delete")]
        [HttpGet]
        public IActionResult Delete([FromRoute] int productId) // mevcut ürün üzerindeki buton ile ürünü silme işi yapılır
        {
            return View();
        }

        [Route("/product/{productId:int}/comment")]
        [HttpPost]
        public IActionResult Comment([FromRoute] int productId, [FromForm] object newProductCommentModel) // ürüne yorum yap butonuyla çalışır.yorum ekler
        {
            // save product comment...

            return RedirectToAction(nameof(HomeController.ProductDetailAsync), "Home", new { productId }); 
        }
    }
}
