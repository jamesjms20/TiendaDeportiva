using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        List<Order> GetOrders();
        List<Order> GetOrdersByperson(int perId);
        Order GetById(int id);
        Order Save(Order order);
        Order Update(Order order);
        bool Delete(int id);

       // Producto AddProduct(Order order,Producto product);
        bool AddProduct(int orId, int pId);

        //bool DeleteProduct(Order order, Producto product);
        bool DeleteProduct(int orId, int pId);

        List<Producto> GetProducts(int orId);



    }
}
