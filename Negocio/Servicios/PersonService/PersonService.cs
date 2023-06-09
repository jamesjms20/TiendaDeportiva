using AccesoDatos.Repositories.PersonRepository;
using Compartido.Entidades;

namespace Negocio.Servicios.PersonService
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        public bool Delete(int id)
        {
            return _personRepository.Delete(id);
        }

        public Person GetByCredentials(string email, string password)
        {
            return _personRepository.GetByCredentials(email, password);
        }

        public Person GetById(int id)
        {
            return _personRepository.GetById(id);
        }

        public Person GetByIdNumber(string idNumber)
        {
            return _personRepository.GetByIdNumber(idNumber);
        }

        public List<Person> GetPersons()
        {
            return _personRepository.GetPersons();
        }

        public Person Save(Person person)
        {
            return _personRepository.Save(person);
        }

        public Person Update(Person person)
        {
            return _personRepository.Update(person);
        }
    }
}
