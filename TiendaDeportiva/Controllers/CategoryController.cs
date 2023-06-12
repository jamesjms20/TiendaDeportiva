using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using TiendaDeportiva.Models;

namespace TiendaDeportiva.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;


        public CategoryController(ILogger<CategoryController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryViewModel> category = new List<CategoryViewModel>();
            try
            {
                string productApiUrl = _configuration["ServicesUrl:Category"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/Category/GetCategories");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        category = JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
            return View(category);
        }

        public async Task<IActionResult> GetByCategory(int id)
        {
            IEnumerable<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                string productApiUrl = _configuration["ServicesUrl:Product"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/product/GetProductByCategoria/" + id);
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


    }
}
