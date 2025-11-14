using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class CommentController : Controller
    {
        [Route("/comment")]
        [HttpGet]
        public IActionResult List()   // admin yorumlar listesini görür 
        {
            return View();
        }

        [Route("/comment/{commentId:int}/approve")]
        [HttpGet]
        public IActionResult Approve([FromRoute] int commentId)  // admin yeni yapılmış yorumları onaylar
        {
            return RedirectToAction(nameof(List));
        }
    }
}
