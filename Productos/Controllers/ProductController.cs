using Compartido.Entidades;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios.ProductService;

namespace Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts/")]
        public  IActionResult GetProducts()
        {
            return Ok(_productService.Productos());
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            return Ok(_productService.GetById(id));
        }

        [HttpGet("GetProductByCategoria/{id}")]
        public IActionResult GetProductByCategoriaId(int id)
        {
            return Ok(_productService.GetproductosByCategoria(id));
        }

        [HttpPost("SaveProduct")]
        public IActionResult SaveProduct(Producto producto)
        {
            return Ok(_productService.Save(producto));
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Producto producto)
        {
            return Ok(_productService.Update(producto));
        }

        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            return Ok(_productService.Delete(id));
        }

      
    }
}
