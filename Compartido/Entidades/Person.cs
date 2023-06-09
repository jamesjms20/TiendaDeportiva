using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.Entidades
{
    public enum PersonType
    {
        Cliente,
        Administrador

    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public PersonType Type { get; set; }

    }
}
