using AccesoDatos.Repositories.OrderRepository;
using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public bool AddProduct(int orId, int pId)
        {
            return _orderRepository.AddProduct(orId, pId);
        }

        public bool Delete(int id)
        {
            return _orderRepository.Delete(id);
        }

        public bool DeleteProduct(int orId, int pId)
        {
            return _orderRepository.DeleteProduct(orId, pId);
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public List<Order> GetOrdersByperson(int perId)
        {
            return _orderRepository.GetOrdersByperson(perId);
        }

        public List<Producto> GetProducts(int orId)
        {
            return _orderRepository.GetProducts(orId);
        }

        public List<Order> GetOrders()
        {
            return _orderRepository.GetOrders();
        }

        public Order Save(Order order)
        {
            return _orderRepository.Save(order);
        }

        public Order Update(Order order)
        {
            return _orderRepository.Update(order);
        }
    }
}
