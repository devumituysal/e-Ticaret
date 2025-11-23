using App.Data.Contexts;
using App.Data.Entities;
using App.e_Ticaret.Models.ViewModels;
using App.Eticaret.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.e_Ticaret.Controllers
{
    public class OrderController : BaseController
    {

        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/order")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CheckoutViewModel model) // sepeti onayla butonu ile çalışır
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = await GetCartItemsAsync();
                return View(viewModel);
            }

            var cartItems = await _dbContext.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                return RedirectToAction(nameof(CartController.EditAsync), "Cart");
            }

            var order = new OrderEntity
            {
                UserId = userId.Value,
                Address = model.Address,
                OrderCode = await CreateOrderCode(),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItemEntity
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Product.Price,
                    CreatedAt = DateTime.UtcNow,
                };

                _dbContext.OrderItems.Add(orderItem);
                _dbContext.CartItems.Remove(cartItem);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(DetailsAsync), new { orderCode = order.OrderCode });
        }

        [Route("/order/{orderId:int}/details")]
        [HttpGet]
        public async Task<IActionResult> DetailsAsync([FromRoute] string orderCode) // sipariş detaylarını gösterir
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var order = await _dbContext.Orders
                .Where(o => o.UserId == userId && o.OrderCode == orderCode)
                .Select(o => new OrderDetailsViewModel
                {
                    OrderCode = o.OrderCode,
                    CreatedAt = o.CreatedAt,
                    Address = o.Address,
                    Items = o.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }

        private async Task<string> CreateOrderCode()
        {
            return Guid.NewGuid().ToString("n")[..16].ToUpperInvariant();
        }

        private async Task<List<CartItemViewModel>> GetCartItemsAsync()
        {
            var userId = GetUserId() ?? -1;

            return await _dbContext.CartItems
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
