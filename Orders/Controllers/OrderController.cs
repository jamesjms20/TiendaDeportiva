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

        [HttpGet("GetOrder/{id}")]
        public IActionResult GetOrder(int id)
        {
            return Ok(_orderService.GetById(id));
        }
        [HttpGet("GetOrderByPerson/{perId}")]
        public IActionResult GetOrderByPerson(int perId)
        {
            return Ok(_orderService.GetOrdersByperson(perId));
        }

    }
}
