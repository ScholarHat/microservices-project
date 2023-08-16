using AuthService.Models;
using AuthService.Services.Interfaces;
using DAL.Database;
using DAL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace AuthService.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        AppDbContext _db;
        IConfiguration _config;
        public AuthenticationService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        public bool CreateUser(User model, string Role)
        {
            try
            {
                model.Password = BC.HashPassword(model.Password);
                Role r = _db.Roles.Where(r => r.Name == Role).FirstOrDefault();
                model.Roles.Add(r);
                _db.Users.Add(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<UserModel> GetUsers()
        {
            var data =  _db.Users.Include(u => u.Roles).Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                Roles = u.Roles.Select(r => r.Name).ToArray()
            });
            return data;
        }

        public UserModel ValidateUser(LoginModel model)
        {
            UserModel user = new UserModel();
            User data = _db.Users.Include(u => u.Roles).Where(u => u.Email == model.Email).FirstOrDefault();
            if (data != null)
            {
                bool isVerifed = BC.Verify(model.Password, data.Password);
                if (isVerifed)
                {
                    user.Id = data.Id;
                    user.Email = data.Email;
                    user.Name = data.Name;
                    user.Roles = data.Roles.Select(r => r.Name).ToArray();
                    user.Token = GenerateJSONWebToken(user);
                    return user;
                }
            }
            return null;
        }

        private string GenerateJSONWebToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                             new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                             new Claim(JwtRegisteredClaimNames.Email, user.Email),
                             new Claim("Roles", string.Join(",",user.Roles)),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                             };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                            _config["Jwt:Audience"],
                                            claims,
                                            expires: DateTime.UtcNow.AddMinutes(60), //token expiry minutes
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
