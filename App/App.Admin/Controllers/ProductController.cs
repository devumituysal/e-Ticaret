using App.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;   
        }

        [Route("/products/")]
        [HttpGet]
        public IActionResult List()   // admin ürünler listesini görür
        {
            return View();
        }

        [Route("/products/filter")]
        [HttpGet]
        public IActionResult Filter([FromQuery] object filterOptions)          // ürünleri filtreler??? 
        {
            // will return filtered products as json
            return Json(new { });
        }

        [Route("/products/{productId:int}/delete")]
        [HttpGet]
        public IActionResult Delete([FromRoute] int productId)  // admin mevcut ürün yanındaki butona basarak ürün siler
        {
            return View();
        }
    }
}
