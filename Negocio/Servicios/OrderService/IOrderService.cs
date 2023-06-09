using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios.OrderService
{
    public interface IOrderService
    {
        List<Order> GetOrders();
        List<Order> GetOrdersByperson(int perId);
        Order GetById(int id);
        Order Save(Order order);
        Order Update(Order order);
        bool Delete(int id);
        bool AddProduct(int orId, int pId);
        bool DeleteProduct(int orId, int pId);

        List<Producto> GetProducts(int orId);
    }
}
