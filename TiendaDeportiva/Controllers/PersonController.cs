using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;
using TiendaDeportiva.Models;

namespace TiendaDeportiva.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonViewModel> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;



        private readonly IConfiguration _configuration;


        public PersonController(ILogger<PersonViewModel> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;


        }


        public async Task<IActionResult> Index()
        {
            // Obtener el objeto person de la sesión
            string personData = HttpContext.Session.GetString("PersonData");
            PersonViewModel person = JsonSerializer.Deserialize<PersonViewModel>(personData, options);
            PersonViewModel personbd = await GetById(person.Id);
           
            return View(personbd);
        }
        [HttpGet]
        public async Task<PersonViewModel> GetById(int id)
        {
            PersonViewModel person = new PersonViewModel();

            try
            {
                string personApiUrl = _configuration["ServicesUrl:Person"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(personApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/Person/Get/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        person = JsonSerializer.Deserialize<PersonViewModel>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return person;
        }


        public async Task<IActionResult> Edit(int id)
        {

            if (id != null)
            {

                PersonViewModel person = await GetById(id);

                return PartialView(person);

            }

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePerson( PersonViewModel model)
        {
            try
            {
                model.Password = "";
                
                string personApiUrl = _configuration["ServicesUrl:Person"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(personApiUrl);

                var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync($"api/Person/Update/", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return RedirectToAction("Index");
                }
                else
                {

                    ModelState.AddModelError("", "Ocurrió un error al actualizar la persona.");
                }
            }
            catch (Exception)
            {
                return NotFound();
            }


            return RedirectToAction("Index");

        }

    }
}
