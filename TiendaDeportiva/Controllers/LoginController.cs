using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using TiendaDeportiva.Models;

namespace TiendaDeportiva.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;


 
        private readonly IConfiguration _configuration;


        public LoginController(ILogger<LoginController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;


        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            PersonViewModel person = new PersonViewModel();
            try
            {
                string PersonApiUrl = _configuration["ServicesUrl:Person"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(PersonApiUrl);

                //   https://localhost:7248/api/Person/GetByCredentials

                HttpResponseMessage response = await httpClient.GetAsync("api/person/GetByCredentials?email="+model.Email+"&password="+model.Password);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content is not null && content != "")
                    {
                        person = JsonSerializer.Deserialize<PersonViewModel>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return NotFound();
            }


            if (person.Password is not null)
            {
                // Guardar la clave en la sesión para indicar que el usuario está autenticado
             
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("typeAuth", person.Type.ToString());

                // Guardar el objeto person en la sesión
                HttpContext.Session.SetString("PersonData", JsonSerializer.Serialize(person));


                // Redirigir a la página de inicio, o a donde desees
                return RedirectToAction("Index", "Person");
            }
            else
            {
                // Las credenciales no son válidas, mostrar un mensaje de error o realizar alguna otra acción
                ModelState.AddModelError(string.Empty, "Credenciales no válidas");
                return View(model);
            }
        }
        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            // Cerrar la sesión actual
           // await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("IsAuthenticated", "false");
            HttpContext.Session.SetString("typeAuth","");
            HttpContext.Session.SetString("PersonData","");


            // Redirigir a la página de inicio u otra página deseada
            return RedirectToAction("Index", "Home");
        }
    }
}
