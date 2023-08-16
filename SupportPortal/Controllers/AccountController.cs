using Microsoft.AspNetCore.Mvc;
using ShoppingUI.Models;
using System.Text;
using System.Text.Json;

namespace SupportPortal.Controllers
{
    public class AccountController : Controller
    {
        HttpClient _client;
        IConfiguration _configuration;
        Uri _baseAddress;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseAddress = new Uri(_configuration["ApiAddress"]);
            _client = new HttpClient();
            _client.BaseAddress = _baseAddress;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            string strData = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");

            var response = _client.PostAsync(_client.BaseAddress + "auth/validateUser", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var strUser = response.Content.ReadAsStringAsync().Result;
                UserModel user = JsonSerializer.Deserialize<UserModel>(strUser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                CookieOptions options = new CookieOptions { Expires = DateTime.Now.AddMinutes(60) };
                Response.Cookies.Append("token", user.Token);

                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return View();
        }
    }
}
