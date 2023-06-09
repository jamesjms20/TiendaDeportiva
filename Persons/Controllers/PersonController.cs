using Compartido.Entidades;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios.PersonService;

namespace Persons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly IPersonService _PersonService;

        public PersonController(IPersonService personService)
        {
            _PersonService = personService;
        }

        [HttpGet("GetPersons/")]
        public IActionResult GetPerosns()
        {
            return Ok(_PersonService.GetPersons());
        }
        [HttpGet("Get/{id}")]
        public IActionResult GetPerson(int id)
        {
            return Ok(_PersonService.GetById(id));
        }
        [HttpGet("GetByIdNumber/{IdNumber}")]
        public IActionResult GetByIdNumber(String IdNumber)
        {
            return Ok(_PersonService.GetByIdNumber(IdNumber)) ;
        }
        [HttpGet("GetByCredentials/")]
        public IActionResult GetByCredentials(String email, String password)
        {
            return Ok(_PersonService.GetByCredentials(email,password));
        }
        [HttpPost("Save/")]
        public IActionResult SavePerson(Person person)
        {
            return Ok(_PersonService.Save(person));
        }
        [HttpPut("Update/")]
        public IActionResult UpdatePerson(Person person)
        {
            return Ok(_PersonService.Update(person));
        }
        [HttpDelete("Delete/")]
        public IActionResult DeletePerson(int id)
        {
            return Ok(_PersonService.Delete(id));
        }


    }
}
