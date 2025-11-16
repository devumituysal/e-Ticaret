using App.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CartController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/add-to-cart/{productId:int}")]
        [HttpGet]
        public IActionResult AddProduct([FromRoute] int productId)  // ürün üzerindeki buton ile sepete ürünü ekleme işini yapar
        {
            // add 1 product...

            var prevUrl = Request.Headers.Referer.FirstOrDefault();

            if (prevUrl is null)
            {
                return RedirectToAction(nameof(Edit));
            }

            return Redirect(prevUrl);
        }
        [Route("/cart")]
        [HttpGet]
        public IActionResult Edit() // mevcut sepeti gösterir
        {
            return View();
        }
        [Route("/cart")]
        [HttpPost]
        public IActionResult Edit([FromForm] object editCartModel) // sepet üzerindeki ürünleri düzenleme işini yapar ( ürün sil , ürün adedi arttır azalt vs)
        {
            return View();
        }
    }
}