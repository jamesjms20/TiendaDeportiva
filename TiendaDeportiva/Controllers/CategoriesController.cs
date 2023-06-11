using Microsoft.AspNetCore.Mvc;

namespace TiendaDeportiva.Controllers
{
    public class CategoriesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
    }
}
