using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        List<Categoria> Categorias();
        Categoria GetById(int catId);
        Categoria Save(Categoria categoria);
        Categoria Update(Categoria categoria);
        bool Delete(int id);
    }
}
