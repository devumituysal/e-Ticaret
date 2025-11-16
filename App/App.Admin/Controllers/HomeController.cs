using App.Admin.Models;
using App.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
