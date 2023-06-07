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
            IEnumerable<Producto> products = _productService.Productos();
            return Ok(products);
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            Producto product = _productService.GetById(id);
            return Ok(product);
        }

        [HttpPost("SaveProduct")]
        public IActionResult SaveProduct(Producto producto)
        {
            Producto product = _productService.Save(producto);
            return Ok(product);
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Producto producto)
        {
            Producto product =  _productService.Update( producto);
            return Ok(product);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            bool product = _productService.Delete(id);
            return Ok(product);
        }

      
    }
}
