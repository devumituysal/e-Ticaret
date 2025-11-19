using App.Data.Contexts;
using App.e_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.e_Ticaret.ViewComponents
{
    public class LatestProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public LatestProductsViewComponent(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = new OwlCarouselViewModel
            {
                Title = "Latest Products",
                Items = await _context.Products
                  .Where(p => p.Enabled)
                  .OrderByDescending(p => p.CreatedAt)
                  .Take(6)
                  .Select(p => new ProductListingViewModel
                  {
                      Id = p.Id,
                      Name = p.Name,
                      Price = p.Price,
                      CategoryName = p.Category.Name,
                      DiscountPercentage = p.Discount == null ? null : p.Discount.DiscountRate,
                      ImageUrl = p.Images.Count != 0 ? p.Images.First().Url : null
                  })
                  .ToListAsync()
            };

            return View(viewModel);
        }
    }
}
