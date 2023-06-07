using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compartido.Entidades;


namespace Negocio.Servicios.ProductService
{
    public interface IProductService
    {
        List<Producto> Productos();
        Producto GetById(int id);
        Producto Save(Producto producto);
        Producto Update(Producto producto);
        bool Delete(int id);
    }
}
