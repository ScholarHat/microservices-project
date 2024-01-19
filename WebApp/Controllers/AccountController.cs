using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        HttpClient _client;
        IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _client = new HttpClient();
            _configuration = configuration;
            _client.BaseAddress = new Uri(_configuration["ApiAddress"]);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var response = _client.PostAsJsonAsync(_client.BaseAddress + "auth/login", model).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var user = JsonSerializer.Deserialize<UserViewModel>(result);
                if (user.Roles.Contains("User"))
                    return RedirectToAction("Index", "Home", new { area = "User" });
            }
            return View();
        }
    }
}
