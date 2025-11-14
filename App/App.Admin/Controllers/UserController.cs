using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class UserController : Controller
    {
        [Route("/users")]
        [HttpGet]
        public IActionResult List() // admin kullanıcıları listeler
        {
            return View();
        }

        [Route("/users/{userId:int}/approve")]
        [HttpGet]
        public IActionResult Approve([FromRoute] int userId)   // admin kullanıcının seller olma isteğini onaylar
        {
            return View();
        }
    }
}
