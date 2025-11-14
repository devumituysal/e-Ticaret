using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class CategoryController : Controller
    {
        [Route("/categories")]
        [HttpGet]
        public IActionResult List()  // admin panelinde kategorileri listeler
        {
            return View();
        }

        [Route("/categories/create")]  
        [HttpGet]
        public IActionResult Create()    // ilgili butona basılınca karegori oluşturma formunu açar
        {
            return View();
        }

        [Route("/categories/create")]    
        [HttpPost]
        public IActionResult Create([FromForm] object newCategoryModel)   // kategori oluşturma formu doldurularak yeni kategori oluşturulur
        {
            return View();
        }

        [Route("/categories/{categoryId:int}/edit")]     
        [HttpGet]
        public IActionResult Edit([FromRoute] int categoryId)    // mevcut kategori üzerindeki buton ile kategoriyi düzenleme formu açılır
        {
            return View();
        }

        [Route("/categories/{categoryId:int}/edit")]
        [HttpPost]
        public IActionResult Edit([FromRoute] int categoryId, [FromForm] object editCategoryModel)  // kategori düzenleme formu doldurularak kategori düzenlenir
        {
            return View();
        }

        [Route("/categories/{categoryId:int}/delete")]     
        [HttpGet]
        public IActionResult Delete([FromRoute] int categoryId)   //mevcut kategori yanındaki butona basılarak kategori silinir
        {
            // delete actions
            return RedirectToAction("Index", "Home");
        }
    }
}
