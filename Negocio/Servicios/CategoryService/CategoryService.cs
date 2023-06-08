using AccesoDatos.Repositories.CategoryRepository;
using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {

            _categoryRepository = categoryRepository;
        }
        public List<Categoria> Categorias()
        {
            return _categoryRepository.Categorias();
        }

        public bool Delete(int id)
        {
            return _categoryRepository.Delete(id);
        }

        public Categoria GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Categoria Save(Categoria categoria)
        {
            return _categoryRepository.Save(categoria);
        }

        public Categoria Update(Categoria categoria)
        {
            return _categoryRepository.Update(categoria);
        }
    }
}
