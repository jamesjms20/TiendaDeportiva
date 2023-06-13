using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using TiendaDeportiva.Models;

namespace TiendaDeportiva.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly IHttpClientFactory _httpClientFactory;


        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                string productApiUrl = _configuration["ServicesUrl:Product"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/product/GetProducts");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        products = JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
            return View(products);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}