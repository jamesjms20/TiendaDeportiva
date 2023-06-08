﻿using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios.CategoryService
{
    public interface ICategoryService
    {
        List<Categoria> Categorias();
        Categoria GetById(int id);
        Categoria Save(Categoria categoria);
        Categoria Update(Categoria categoria);
        bool Delete(int id);
    }
}
