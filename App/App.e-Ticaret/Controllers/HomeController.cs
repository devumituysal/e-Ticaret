using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Repositories.Interfaces;
using App.e_Ticaret.Models.ViewModels;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace App.e_Ticaret.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataRepository<ProductEntity> _prRepo;
        private readonly IDataRepository<ContactFormEntity> _cfRepo;

        public HomeController(IDataRepository<ProductEntity> prRepo, IDataRepository<ContactFormEntity> cfRepo)
        {
            _prRepo = prRepo;
            _cfRepo = cfRepo;
        }

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
        public async Task<IActionResult> ContactAsync([FromForm] NewContactFormMessageViewModel newContactMessage) // iletiþim formu doldurulduktan sonra gönderilme iþini yapar
        {
            if (!ModelState.IsValid)
            {
                return View(newContactMessage);
            }

            var contactMessageEntity = new ContactFormEntity
            {
                Name = newContactMessage.Name,
                Email = newContactMessage.Email,
                Message = newContactMessage.Message,
                CreatedAt = DateTime.UtcNow,
                SeenAt = null
            };

            await _cfRepo.AddAsync(contactMessageEntity);

            ViewBag.SuccessMessage = "Your message has been sent successfully.";

            return View();
        }
        [Route("/product/list")]
        [HttpGet]
        public async Task<IActionResult> Listing()  // ürünler sayfasýný açar
        {
            var products = await _prRepo.GetAll()
                .Where(p => p.Enabled)
                .Select(p => new ProductListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    DiscountPercentage = p.Discount == null ? null : p.Discount.DiscountRate,
                    ImageUrl = p.Images.Count != 0 ? p.Images.First().Url : null
                })
                .ToListAsync();

            return View(products);
        }
        [Route("/product/{productId:int}/details")]
        [HttpGet]
        public async Task<IActionResult> ProductDetailAsync([FromRoute] int productId) // bir ürünün üzerindeki buton ile çalýþýr ve detaylarýný gösterir
        {
            var product = await _prRepo.GetAll()
                .Where(p => p.Enabled && p.Id == productId)
                .Select(p => new HomeProductDetailViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DiscountRate = p.Discount == null ? null : p.Discount.DiscountRate,
                    Description = p.Details,
                    StockAmount = p.StockAmount,
                    SellerName = p.Seller.FirstName + " " + p.Seller.LastName,
                    CategoryName = p.Category.Name,
                    CategoryId = p.CategoryId,
                    ImageUrls = p.Images.Select(i => i.Url).ToArray(),
                    Reviews = p.Comments.Where(c => c.IsConfirmed) // show only confirmed comments
                        .Select(c => new ProductReviewViewModel
                        {
                            Id = c.Id,
                            Text = c.Text,
                            StarCount = c.StarCount,
                            UserName = c.User.FirstName + " " + c.User.LastName
                        }).ToArray()
                })
                .FirstOrDefaultAsync();

            if (product is null)
            {
                return NotFound();
            }


            return View(product);
        }
    }
}
