using App.Data.Contexts;
using App.e_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.e_Ticaret.ViewComponents
{
    public class CategoriesSliderViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoriesSliderViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Products
                .Include(p=>p.Category)
                .GroupBy(p=>p.CategoryId)
                .Select(g => new CategorySliderViewModel
                {
                    Id = g.First().Category.Id,
                    Name = g.First().Category.Name,
                    Color = g.First().Category.Color,
                    IconCssClass = g.First().Category.IconCssClass,
                    ImageUrl = g.First().Images.Count != O ? g.First().Images.First().Url : null
                    
                })
                .ToListAsync();

            return View(categories);

        }
    }
}
