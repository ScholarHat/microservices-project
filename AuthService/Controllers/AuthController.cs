using AuthService.Models;
using AuthService.Services.Interfaces;
using DAL.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthenticationService _authService;
        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IEnumerable<UserModel> GetUsers()
        {

            return _authService.GetUsers();
        }

        [HttpPost]
        public IActionResult CreateUser(SignUpModel model)
        {
            User user = new User
            {
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password
            };
            var result = _authService.CreateUser(user, model.Role);
            if (result)
            {
                UserModel userModel = new UserModel
                {
                    Id = user.Id,
                    Name = model.Name,
                    Email = model.Email,
                    Roles = user.Roles.Select(r => r.Name).ToArray()
                };
                return CreatedAtAction("CreateUser", userModel);
            }    
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult ValidateUser(LoginModel model)
        {
            UserModel user = _authService.ValidateUser(model);
            if (user != null)
                return Ok(user);
            else
                return NoContent();
        }
    }
}
