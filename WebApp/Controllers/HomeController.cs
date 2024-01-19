using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        HttpClient _client;
        IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _client = new HttpClient();
            _configuration = configuration;
            _client.BaseAddress = new Uri(_configuration["ApiAddress"]);
        }

        public IActionResult Index()
        {
            IEnumerable<ProductViewModel> products = null;
            var response = _client.GetAsync(_client.BaseAddress + "catalog/getproducts").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                products = JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(result);
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
