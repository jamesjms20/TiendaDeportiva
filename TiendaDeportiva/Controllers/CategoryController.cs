using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;
using System.Text;
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
                string categoryApiUrl = _configuration["ServicesUrl:Category"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(categoryApiUrl);

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

        [HttpGet]
        public async Task<CategoryViewModel> GetById(int id)
        {
            CategoryViewModel category = new CategoryViewModel();

            try
            {
                string categoryApiUrl = _configuration["ServicesUrl:Category"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(categoryApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/Category/GetCategory/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        category = JsonSerializer.Deserialize<CategoryViewModel>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return category;
        }

    
        [HttpPost]
        public async Task<IActionResult> AddCategory(string nombre, string descripcion)
        {
            CategoryViewModel model = new CategoryViewModel();
            try
            {
                model.Id = 0;
                model.Nombre = nombre;
                model.Descripcion=descripcion;

                string categoryApiUrl = _configuration["ServicesUrl:Category"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(categoryApiUrl);

                var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync($"api/Category/SaveCategory/", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                   
                    return RedirectToAction("AdminCategory");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error al agregar la categoría.");
                }
            }
            catch (Exception)
            {
                return NotFound();
            }


            return RedirectToAction("AdminCategory");


        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string categoryApiUrl = _configuration["ServicesUrl:Category"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(categoryApiUrl);

                HttpResponseMessage response = await httpClient.DeleteAsync("api/Category/DeleteCategory/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool result;
                    if (bool.TryParse(content, out result))
                    {
                        if (result)
                        {

                            return Content("<script>window.location.reload();</script>");
                        }

                    }
                }
            }
            catch (Exception)
            {
                return Content("<script>window.location.reload();</script>");
            }
            return Content("<script>window.location.reload();</script>");
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

        public async Task<IActionResult> AdminCategory()
        {
            IEnumerable<CategoryViewModel> categories = new List<CategoryViewModel>();
            try
            {
                string categoryApiUrl = _configuration["ServicesUrl:Category"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(categoryApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/Category/GetCategories");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        categories = JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
            return View(categories);
        }

        public async Task<IActionResult> Edit(int id)
        {

            if (id != null)
            {

                CategoryViewModel category = await GetById(id);
                return PartialView(category);

            }

            return PartialView();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int id, CategoryViewModel model)
        {

            try
            {
                model.Id = id;
                string categoryApiUrl = _configuration["ServicesUrl:Category"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(categoryApiUrl);

                var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"api/Category/UpdateCategory/", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AdminCategory");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la categoría.");
                }
            }
            catch (Exception)
            {
                return NotFound();
            }


            return RedirectToAction("AdminCategory");

        }
   
    }
}
