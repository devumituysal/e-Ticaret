using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Repositories.Implementations;
using App.Data.Repositories.Interfaces;
using App.e_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.e_Ticaret.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IDataRepository<UserEntity> _usRepo;
        private readonly IDataRepository<OrderEntity> _orRepo;
        private readonly IDataRepository<ProductEntity> _prRepo;

        public ProfileController(IDataRepository<UserEntity> usRepo,
        IDataRepository<OrderEntity> orRepo,
        IDataRepository<ProductEntity> prRepo)
        {
            _usRepo = usRepo;
            _orRepo = orRepo;
            _prRepo = prRepo;
        }

        [Route("/profile")]
        [HttpGet]
        public async Task<IActionResult> DetailsAsync() // kullanıcı kendi profil sayfasını görür
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userViewModel = await _usRepo.GetAll()
                .Where(u => u.Id == userId.Value)
                .Select(u => new ProfileDetailsViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();

            if (userViewModel is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string? previousSuccessMessage = TempData["SuccessMessage"]?.ToString();

            if (previousSuccessMessage is not null)
            {
                SetSuccessMessage(previousSuccessMessage);
            }

            return View(userViewModel);
        }

        [Route("/profile")]
        [HttpPost]
        public async Task<IActionResult> EditAsync([FromForm] ProfileDetailsViewModel editMyProfileModel) // kullanıcı kendi sayfasını günceller
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await GetCurrentUserAsync();

            if (user is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(editMyProfileModel);
            }

            user.FirstName = editMyProfileModel.FirstName;
            user.LastName = editMyProfileModel.LastName;

            if (!string.IsNullOrWhiteSpace(editMyProfileModel.Password) && editMyProfileModel.Password != "******")
            {
                user.Password = editMyProfileModel.Password;
            }

            await _usRepo.UpdateAsync(user);

            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";

            return RedirectToAction(nameof(DetailsAsync));
        }

        [Route("/my-orders")]
        [HttpGet]
        public async Task<IActionResult> MyOrdersAsync() // geçmiş siparişleri gösterir
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<OrderViewModel> orders = await _orRepo.GetAll()
                .Where(o => o.UserId == userId.Value)
                .Select(o => new OrderViewModel
                {
                    OrderCode = o.OrderCode,
                    Address = o.Address,
                    CreatedAt = o.CreatedAt,
                    TotalPrice = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                    TotalProducts = o.OrderItems.Count,
                    TotalQuantity = o.OrderItems.Sum(oi => oi.Quantity),
                })
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        [Route("/my-products")]
        [HttpGet]
        public async Task<IActionResult> MyProductsAsync() // satıcı ise mevcut ürünlerini gösterir
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await IsUserSellerAsync())
            {
                return RedirectToAction("Index", "Home");
            }

            List<MyProductsViewModel> products = await _prRepo.GetAll()
                .Where(p => p.SellerId == userId.Value)
                .Select(p => new MyProductsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Details,
                    Stock = p.StockAmount,
                    HasDiscount = p.DiscountId != null,
                    CreatedAt = p.CreatedAt,
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(products);
        }
    }
}
