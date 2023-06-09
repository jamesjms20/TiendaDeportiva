using Compartido.Entidades;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios.OrderService;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetOrders/")]
        public IActionResult GetOrders()
        {
            return Ok(_orderService.Orders());
        }

        [HttpGet("Get/{id}")]
        public IActionResult GetOrder(int id)
        {
            return Ok(_orderService.GetById(id));
        }
        [HttpGet("GetByPerson/{perId}")]
        public IActionResult GetOrderByPerson(int perId)
        {
            return Ok(_orderService.GetOrdersByperson(perId));
        }
        [HttpPost("Save/")]
        public IActionResult SaveOrder(Order order)
        {
            return Ok(_orderService.Save(order));
        }
        [HttpPut("Update/")]
        public IActionResult UpdateOrder(Order order)
        {
            return Ok(_orderService.Update(order));
        }
        [HttpDelete("Delete/")]
        public IActionResult DeleteOrder(int id)
        {
            return Ok(_orderService.Delete(id));
        }
        [HttpPost("SaveProduct/")]
        public IActionResult SaveProductOrder(int orId, int pId)
        {
            return Ok(_orderService.AddProduct(orId, pId));
        }
        [HttpDelete("DeleteProduct/")]
        public IActionResult DeleteProductOrder(int orId, int pId)
        {
            return Ok(_orderService.DeleteProduct(orId, pId));
        }
        [HttpGet("GetProducts/{orID}")]
        public IActionResult GetProductsOrder(int orId)
        {
            return Ok(_orderService.GetProducts(orId));
        }


    }
}
