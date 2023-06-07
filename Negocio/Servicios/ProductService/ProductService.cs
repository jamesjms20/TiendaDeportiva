using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Repositories.ProductRepository;

namespace Negocio.Servicios.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productoRepository)
        {
            _productRepository = productoRepository;
        }

        public bool Delete(int id)
        {
            return _productRepository.Delete(id);

        }

        public Producto GetById(int id)
        {
           return _productRepository.GetById(id);
        }

        public List<Producto> Productos()
        {
            return _productRepository.Productos();
        }

        public Producto Save(Producto producto)
        {
            return _productRepository.Save(producto);
        }

        public Producto Update(Producto producto)
        {
            return _productRepository.Update(producto);
        }
    }
}
