using Microsoft.AspNetCore.Mvc;
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


        public async Task<IActionResult> Index(PersonViewModel person)
        {
            ////PersonViewModel person;
            //try
            //{
            //    string personApiUrl = _configuration["ServicesUrl:Person"];
            //    HttpClient httpClient = _httpClientFactory.CreateClient();
            //    httpClient.BaseAddress = new Uri(personApiUrl);

            //    HttpResponseMessage response = await httpClient.GetAsync("api/person/GetPersons");
            //    if (response.IsSuccessStatusCode)
            //    {
            //        string content = await response.Content.ReadAsStringAsync();
            //        if (content != null)
            //        {
            //            person = JsonSerializer.Deserialize<IEnumerable<PersonViewModel>>(content, options);
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    return NotFound();
            //}
            return View(person);
        }
    }
}
