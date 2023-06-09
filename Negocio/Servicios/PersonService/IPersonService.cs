using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios.PersonService
{
    public interface IPersonService
    {
        List<Person> GetPersons();
        Person GetById(int id);

        Person GetByIdNumber(string idNumber);
        Person GetByCredentials(string email, string password);
        Person Save(Person person);
        Person Update(Person person);
        bool Delete(int id);
    }
}
