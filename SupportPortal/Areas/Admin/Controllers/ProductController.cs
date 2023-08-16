using Microsoft.AspNetCore.Mvc;
using ShoppingUI.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SupportPortal.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        HttpClient _client;
        IConfiguration _configuration;
        Uri _baseAddress;
        public ProductController(IConfiguration configuration)
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
        public IEnumerable<CategoryModel> GetCategories()
        {
            IEnumerable<CategoryModel> model = new List<CategoryModel>();

            var response = _client.GetAsync(_client.BaseAddress + "catalog/getcategories").Result;
            if (response.IsSuccessStatusCode)
            {
                var strUser = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<IEnumerable<CategoryModel>>(strUser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return model;
        }
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductModel model)
        {
            var token = Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string strData = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");

            var response = _client.PostAsync(_client.BaseAddress + "catalog/addproduct", content).Result;
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index", "Product");
            }
            return View();
        }
    }
}
