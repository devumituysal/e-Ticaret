using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Repositories.Interfaces;
using App.e_Ticaret.Controllers;
using App.e_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    public class CartController : BaseController
    {
        private readonly IDataRepository<ProductEntity> _productRepo;
        private readonly IDataRepository<CartItemEntity> _ciRepo;

        public CartController(IDataRepository<ProductEntity> productRepo, IDataRepository<CartItemEntity> ciRepo)
        {
            _productRepo = productRepo;
            _ciRepo = ciRepo;
        }

        [Route("/add-to-cart/{productId:int}")]
        [HttpGet]
        public async Task<IActionResult> AddProduct([FromRoute] int productId)  // ürün üzerindeki buton ile sepete ürünü ekleme işini yapar
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            if (!await _productRepo.GetAll().AnyAsync(p => p.Id == productId))
            {
                return NotFound();
            }

            var cartItem = await _ciRepo.GetAll().FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

            if (cartItem is not null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new CartItemEntity
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedAt = DateTime.UtcNow
                };

                await _ciRepo.AddAsync(cartItem);
            }


            var prevUrl = Request.Headers.Referer.FirstOrDefault();

            if (prevUrl is null)
            {
                return RedirectToAction(nameof(Edit));
            }

            return Redirect(prevUrl);
        }
        [Route("/cart")]
        [HttpGet]
        public async Task<IActionResult> Edit() // mevcut sepeti gösterir
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            List<CartItemViewModel> cartItem = await GetCartItemsAsync();

            return View(cartItem);
        }

        [HttpGet("/cart/{cartItemId:int}/remove")]
        public async Task<IActionResult> Remove([FromRoute] int cartItemId)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var cartItem = await _ciRepo.GetAll().FirstOrDefaultAsync(ci => ci.UserId == userId && ci.Id == cartItemId);

            if (cartItem is null)
            {
                return NotFound();
            }

            await _ciRepo.DeleteAsync(cartItemId);


            return RedirectToAction(nameof(Edit));
        }

        [HttpPost("/cart/update")]
        public async Task<IActionResult> UpdateCart(int cartItemId, byte quantity)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var cartItem = await _ciRepo.GetAll()
                .Include(ci => ci.Product.Images)
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.Id == cartItemId);

            if (cartItem is null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            cartItem = await _ciRepo.UpdateAsync(cartItem)!;

            var model = new CartItemViewModel
            {
                Id = cartItem.Id,
                ProductName = cartItem.Product.Name,
                ProductImage = cartItem.Product.Images.Count != 0 ? cartItem.Product.Images.First().Url : null,
                Quantity = cartItem.Quantity,
                Price = cartItem.Product.Price
            };

            return View(model);
        }

        [HttpGet("/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            List<CartItemViewModel> cartItems = await GetCartItemsAsync();

            return View(cartItems);
        }

        private async Task<List<CartItemViewModel>> GetCartItemsAsync()
        {
            var userId = GetUserId() ?? -1;

            return await _ciRepo.GetAll()
                .Include(ci => ci.Product.Images)
                .Where(ci => ci.UserId == userId)
                .Select(ci => new CartItemViewModel
                {
                    Id = ci.Id,
                    ProductName = ci.Product.Name,
                    ProductImage = ci.Product.Images.Count != 0 ? ci.Product.Images.First().Url : null,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                })
                .ToListAsync();
        }
    }
}