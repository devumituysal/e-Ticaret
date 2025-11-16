using App.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace App.e_Ticaret.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/order")]
        [HttpPost]
        public IActionResult Create() // sepeti onayla butonu ile çalışır
        {
            // create order...
            var orderId = 1;
            return RedirectToAction(nameof(Details), new { orderId });
        }

        [Route("/order/{orderId:int}/details")]
        [HttpGet]
        public IActionResult Details([FromRoute] int orderId) // sipariş detaylarını gösterir
        {
            return View();
        }
    }
}
