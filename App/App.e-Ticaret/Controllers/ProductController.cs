using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Repositories.Implementations;
using App.Data.Repositories.Interfaces;
using App.e_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace App.e_Ticaret.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IDataRepository<ProductEntity> _prRepo;
        private readonly IDataRepository<ProductImageEntity> _priRepo;
        private readonly IDataRepository<ProductCommentEntity> _prcRepo;

        public ProductController(IDataRepository<ProductEntity> prRepo,
        IDataRepository<ProductImageEntity> priRepo,
        IDataRepository<ProductCommentEntity> prcRepo)
        {
            _prRepo = prRepo;
            _priRepo = priRepo;
            _prcRepo = prcRepo;

        }


        [Route("/product")]
        [HttpGet]
        public  IActionResult Create() // yeni bir ürün ekleme formunu açar ( muhtemel senaryo sadece satıcılar için)
        {
            return View();
        }

        [Route("/product")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EditProductViewModel newProductModel) // form doldurularak yeni ürün ekleme işini yapar
        {

            if (!ModelState.IsValid)
            {
                return View(newProductModel);
            }

            var productEntity = new ProductEntity
            {
                SellerId = 2, // TODO: User'ı al
                CategoryId = newProductModel.CategoryId,
                DiscountId = newProductModel.DiscountId,
                Name = newProductModel.Name,
                Price = newProductModel.Price,
                Details = newProductModel.Description,
                StockAmount = newProductModel.StockAmount,
                CreatedAt = DateTime.UtcNow

            };

            productEntity = await _prRepo.AddAsync(productEntity);

            await SaveProductImages(productEntity.Id, newProductModel.Images);

            ViewBag.SuccessMessage = "Ürün başarıyla eklendi.";
            ModelState.Clear();

            return View();

           
        }

        private async Task SaveProductImages(int productId, IList<IFormFile> images)
        {
            foreach (var image in images)
            {
                var productImageEntity = new ProductImageEntity
                {
                    ProductId = productId,
                    Url = $"/uploads/{Guid.NewGuid()}{Path.GetExtension(image.FileName)}"
                };

                productImageEntity = await _priRepo.AddAsync(productImageEntity);

                await using var fileStream = new FileStream($"wwwroot{productImageEntity.Url}", FileMode.Create);
                await image.CopyToAsync(fileStream);
            }
        }

        [Route("/product/{productId:int}/edit")]
        [HttpGet]
        public async Task<IActionResult> EditAsync([FromRoute] int productId, [FromForm] EditProductViewModel editProductModel) // mevcut ürün üzerindeki buton ile edit sayfasını açar ( yada ürün detayları)
        {
            var productEntity = await _prRepo.GetByIdAsync(productId);

            if (productEntity is null)
            {
                return NotFound();
            }

            var viewModel = new EditProductViewModel
            {
                CategoryId = productEntity.CategoryId,
                DiscountId = productEntity.DiscountId,
                Name = productEntity.Name,
                Price = productEntity.Price,
                Description = productEntity.Details,
                StockAmount = productEntity.StockAmount
            };

            return View(viewModel);
        }

        [Route("/product/{productId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int productId, [FromForm] EditProductViewModel editProductModel) // form doldurularak ürün detayları güncellenir
        {
            if (!ModelState.IsValid)
            {
                return View(editProductModel);
            }

            var productEntity = await _prRepo.GetByIdAsync(productId);

            if (productEntity is null)
            {
                return NotFound();
            }

            productEntity.CategoryId = editProductModel.CategoryId;
            productEntity.DiscountId = editProductModel.DiscountId;
            productEntity.Name = editProductModel.Name;
            productEntity.Price = editProductModel.Price;
            productEntity.Details = editProductModel.Description;
            productEntity.StockAmount = editProductModel.StockAmount;

            await _prRepo.UpdateAsync(productEntity);

            ViewBag.SuccessMessage = "Ürün başarıyla güncellendi.";

            return View(editProductModel);
        }

        [Route("/product/{productId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int productId) // mevcut ürün üzerindeki buton ile ürünü silme işi yapılır
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (!await _prRepo.GetAll().AnyAsync(x => x.Id == productId && x.SellerId == userId))
            {
                return NotFound();
            }

            await _prRepo.DeleteAsync(productId);

            ViewBag.SuccessMessage = "Ürün başarıyla silindi.";

            return View();
        }

        [Route("/product/{productId:int}/comment")]
        [HttpPost]
        public async Task<IActionResult> Comment([FromRoute] int productId, [FromForm] EditProductCommentViewModel newProductCommentModel) // ürüne yorum yap butonuyla çalışır.yorum ekler
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _prRepo.GetAll().AnyAsync(x => x.Id == productId))
            {
                return NotFound();
            }

            if (await _prcRepo.GetAll().AnyAsync(x => x.ProductId == productId && x.UserId == userId))
            {
                return BadRequest();
            }

            var productCommentEntity = new ProductCommentEntity
            {
                ProductId = productId,
                UserId = userId.Value,
                Text = newProductCommentModel.Text,
                StarCount = newProductCommentModel.StarCount,
                CreatedAt = DateTime.UtcNow,
            };

            await _prcRepo.AddAsync(productCommentEntity);

            return Ok();
        }
    }
}
