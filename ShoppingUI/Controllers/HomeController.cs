using Microsoft.AspNetCore.Mvc;
using ShoppingUI.Models;
using System.Diagnostics;
using System.Text.Json;

namespace ShoppingUI.Controllers
{
    public class HomeController : Controller
    {
        HttpClient _client;
        IConfiguration _configuration;
        Uri _baseAddress;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseAddress = new Uri(_configuration["ApiAddress"]);
            _client = new HttpClient();
            _client.BaseAddress = _baseAddress;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductModel> model = new List<ProductModel>();

            var response = _client.GetAsync(_client.BaseAddress + "catalog/getproducts").Result;
            if (response.IsSuccessStatusCode)
            {
                var strUser = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<IEnumerable<ProductModel>>(strUser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return View(model);
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