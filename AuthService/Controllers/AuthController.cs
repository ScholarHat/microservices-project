using AuthService.Model;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthServiceRepository _authService;
        public AuthController(IAuthServiceRepository authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                UserModel user = _authService.ValidateUser(model.Email, model.Password);
                if (user != null)
                {
                    return Ok(user);
                }
                return BadRequest("Invalid credentials");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SignUp(SignUpModel user)
        {
            try
            {
                bool result = _authService.CreateUser(user);
                if (result)
                {
                    return Ok("User created successfully");
                }
                return BadRequest("User creation failed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
