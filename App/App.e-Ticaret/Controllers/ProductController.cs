using App.Data.Contexts;
using App.e_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create() // yeni bir ürün ekleme formunu açar ( muhtemel senaryo sadece satıcılar için)
        {
            ViewBag.Discounts = await _dbContext.ProductDiscounts
                .Where(d=>d.Enabled)
                .Select(d=> new DiscountSelectItemViewModel { Id = d.Id, Rate = d.DiscountRate })
                .ToListAsync();

            ViewBag.Categories = await _dbContext.Categories
                .Select(d => new CategorySelectItemViewModel { Id = d.Id , Name = d.Name})
                .ToListAsync();
            return View();
        }

        [Route("/product")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] EditProductViewModel newProductModel) // form doldurularak yeni ürün ekleme işini yapar
        {
            ViewBag.Discounts = await _dbContext.ProductDiscounts
                 .Where(d => d.Enabled)
                 .Select(d => new DiscountSelectItemViewModel { Id = d.Id, Rate = d.DiscountRate })
                 .ToListAsync();

            ViewBag.Categories = await _dbContext.Categories
                .Select(c => new CategorySelectItemViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(newProductModel);
            }

            // TODO: save new product...

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
