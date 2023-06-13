using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
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


        public async Task<IActionResult> Index(int id)
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
                        products = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(content, options);
                    }
                }
                //if (products.Count() == 0) {
                //    return PartialView(id);
                //}
            }
            catch (Exception)
            {
                return NotFound();
            }
            return PartialView(products);
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
        [HttpGet]
        public async Task<ProductViewModel> GetById(int id)
        {
            ProductViewModel product = new ProductViewModel();
            try
            {
                string productApiUrl = _configuration["ServicesUrl:Product"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);

                HttpResponseMessage response = await httpClient.GetAsync("api/Product/GetProduct/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        product = System.Text.Json.JsonSerializer.Deserialize<ProductViewModel>(content, options);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return product;
        }

        public async Task<IActionResult> Edit(int id)
        {

            if (id != null)
            {

                ProductViewModel product = await GetById(id);
                return PartialView(product);

            }

            return PartialView();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, ProductViewModel model)
        {

            try
            {
                model.Id = id;
                string productApiUrl = _configuration["ServicesUrl:Product"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"api/Product/UpdateProduct/", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("adminCategory","Category");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                   
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el producto.");
                }
            }
            catch (Exception)
            {
                return NotFound();
            }


            return RedirectToAction("adminCategory", "Category");

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string productApiUrl = _configuration["ServicesUrl:Product"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);

                HttpResponseMessage response = await httpClient.DeleteAsync("api/Product/DeleteProduct/" + id);
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

        [HttpPost]
        public async Task<IActionResult> AddProduct(string nombre, int catId, string descripcion,int precio)
        {
            ProductViewModel model = new ProductViewModel();
            try
            {
                model.Id = 0;
                model.Nombre = nombre;
                model.CatId = catId;
                model.Precio = precio;
                model.Descripcion = descripcion;

                string productApiUrl = _configuration["ServicesUrl:Product"];
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(productApiUrl);
                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync($"api/Product/SaveProduct/", jsonContent);
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
                // Ocurrió una excepción durante la comunicación con el API
                return NotFound();
            }


            return RedirectToAction("AdminCategory");


        }






        [HttpPost]
        public IActionResult AddToCartByCart(string item)
        {
            //IActionResult result;
            if (!string.IsNullOrEmpty(item))
            {
                ProductViewModel product = JsonConvert.DeserializeObject<ProductViewModel>(item);

                if (product != null)
                {

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
                // Almacenar un mensaje de éxito en TempData
                TempData["SuccessMessage"] = "El producto se agregó correctamente al carrito.";

                // Redirigir a la página de productos o mostrar un mensaje de éxito
                //return Ok();//RedirectToAction("Index");
            }
            return RedirectToAction("ViewCart");

        }

        public IActionResult AddToCart(ProductViewModel product)
        {

            if (product != null)
            {
   
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
            // Almacenar un mensaje de éxito en TempData
            TempData["SuccessMessage"] = "El producto se agregó correctamente al carrito.";

            // Redirigir a la página de productos o mostrar un mensaje de éxito
            return RedirectToAction("Index");
        }

        public IActionResult DeleteToCart(string item)
        {

            if (!string.IsNullOrEmpty(item))
            {
                ProductViewModel product = JsonConvert.DeserializeObject<ProductViewModel>(item);

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
                    existingItem.Quantity -= 1;
                    if(existingItem.Quantity <= 0)
                    {
                        cart.Remove(existingItem);
                    }
                }
                else
                {
                    // Si el producto no existe, agregarlo al carrito
                    //cart.Add(new CartItem
                    //{
                    //    Product = product,
                    //    Quantity = 1
                    //});
                }

                // Guardar el carrito en la sesión del usuario
                HttpContext.Session.SetObject("Cart", cart);
            }

            // Redirigir a la página de productos o mostrar un mensaje de éxito
            // Almacenar un mensaje de éxito en TempData
            TempData["SuccessMessage"] = "El producto se eliminó correctamente del carrito.";

            return RedirectToAction("ViewCart");
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
