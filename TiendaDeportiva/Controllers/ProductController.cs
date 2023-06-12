using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TiendaDeportiva.Extensions;
using TiendaDeportiva.Models;

namespace TiendaDeportiva.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;


        //public HomeController(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClientFactory = httpClientFactory;
        //}
        private readonly IConfiguration _configuration;


        public ProductController(ILogger<ProductController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;


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
            return PartialView(products);
        }

        //       private ProductViewModel
        [HttpPost]
        public IActionResult AddToCart(ProductViewModel product)
        {
            if (product != null)
            {
                // product = JsonSerializer.Deserialize<ProductViewModel>(productJson);

                // Obtener el carrito de la sesión del usuario o crear uno nuevo
                List<CartItem> cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                // Verificar si el producto ya existe en el carrito
                CartItem existingItem = cart.FirstOrDefault(item => item.Product.Id == product.Id);
                if (existingItem != null)
                {
                    // Si el producto ya existe, actualizar la cantidad
                    existingItem.Quantity += 1;
                }
                else
                {
                    // Si el producto no existe, agregarlo al carrito
                    cart.Add(new CartItem
                    {
                        Product = product,
                        Quantity = 1
                    });
                }

                // Guardar el carrito en la sesión del usuario
                HttpContext.Session.SetObject("Cart", cart);
            }

            // Redirigir a la página de productos o mostrar un mensaje de éxito
            return RedirectToAction("Index");
        }


        public IActionResult ViewCart()
        {
            List<CartItem> cart = _httpContextAccessor.HttpContext.Session.GetObject<List<CartItem>>("Cart");

            return View(cart);
        }


        public IActionResult Checkout()
        {
            // Procesar el carrito y realizar la compra
            // ...

            // Vaciar el carrito
            HttpContext.Session.Remove("Cart");

            // Redirigir a la página de confirmación o mostrar un mensaje de éxito
            return RedirectToAction("Confirmation");
        }


    }
}
