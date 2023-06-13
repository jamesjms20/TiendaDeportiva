using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using TiendaDeportiva.Models;

namespace TiendaDeportiva.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderViewModel> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;


        public OrderController(ILogger<OrderViewModel> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            IEnumerable<OrderViewModel> Orders = new List<OrderViewModel>();
            try
            {
                string OrderApiUrl = _configuration["ServicesUrl:Order"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(OrderApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/Order/GetOrders");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        Orders = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<OrderViewModel>>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
            return View(Orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsOrder(int id)
        {
            IEnumerable<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                string productApiUrl = _configuration["ServicesUrl:Order"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/Order/GetProducts?orId=" + id);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        products = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(content, options);
                    }
                }

            }
            catch (Exception)
            {
                return NotFound();
            }
            return PartialView(products);
        }

        public IActionResult Pay(string cartItems)
        {

            string personData = HttpContext.Session.GetString("PersonData");
            if (personData == null)
            {

                return RedirectToAction("Index", "Login");
            }
            else
            {
                PersonViewModel person = System.Text.Json.JsonSerializer.Deserialize<PersonViewModel>(personData, options);


                List<CartItem> cart = JsonConvert.DeserializeObject<List<CartItem>>(cartItems);
                List<ProductViewModel> products = new List<ProductViewModel>();
                foreach (var cartItem in cart)
                {
                    ProductViewModel producto = cartItem.Product;

                    for (int i = 0; i < cartItem.Quantity; i++)
                    {
                        products.Add(producto);
                    }

                }

                OrderViewModel order = new OrderViewModel();

                order.Id = 0;
                order.perId = person.Id;
                order.Date = DateTime.Now;
                order.Status = "realizada";
                order.Products = products;

                AddOrder(order);

                HttpContext.Session.Remove("Cart");

                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderViewModel model)
        {
            try
            {
                string orderApiUrl = _configuration["ServicesUrl:Order"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(orderApiUrl);
                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync($"api/Order/Save/", jsonContent);
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
                    ModelState.AddModelError("", "Ocurrió un error alagregar el producto");
                }
            }
            catch (Exception)
            {
                return NotFound();
            }


            return RedirectToAction("ViewCart", "Product"); ;


        }

    }
}
