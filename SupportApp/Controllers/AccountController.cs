using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Models;
using System.Security.Claims;
using System.Text.Json;

namespace SupportApp.Controllers
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
        async void GenerateTicket(UserViewModel user)
        {
            string strData = JsonSerializer.Serialize(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, strData),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, String.Join(",", user.Roles))
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var response = _client.PostAsJsonAsync(_client.BaseAddress + "auth/login", model).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var user = JsonSerializer.Deserialize<UserViewModel>(result);
                GenerateTicket(user);

                if (user.Roles.Contains("Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View();
        }
    }
}
