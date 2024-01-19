using Microsoft.AspNetCore.Mvc;
using SupportApp.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SupportApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        HttpClient _client;
        IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _client = new HttpClient();
            _configuration = configuration;
            _client.BaseAddress = new Uri(_configuration["ApiAddress"]);
        }

        public IActionResult Index()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.Token);
            var response = _client.GetAsync(_client.BaseAddress + "product/GetProducts").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var products = JsonSerializer.Deserialize<List<ProductViewModel>>(result);
                return View(products);
            }
            return View();
        }
    }
}
