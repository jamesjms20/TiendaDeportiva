using Compartido.Entidades;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios.CategoryService;

namespace Categorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("GetCategories")]
        public IActionResult GetCategories() {
            return Ok(_categoryService.Categorias());
        }
        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategory(int id) {
            return Ok(_categoryService.GetById(id));
        }
        [HttpPost("SaveCategory/")]
        public IActionResult saveCategory(Categoria categoria) {
            return Ok(_categoryService.Save(categoria));
        }
        [HttpPut("UpdateCategory/")]
        public IActionResult UpdateCategory(Categoria categoria) { 
        return Ok(_categoryService.Update(categoria));
        }
        [HttpDelete("DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int id) {
            return Ok(_categoryService.Delete(id));

        }


    }
}
