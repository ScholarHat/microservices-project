using AuthService.Database;
using AuthService.Database.Entities;
using AuthService.Model;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services.Implementation
{
    public class AuthServiceRepository : IAuthServiceRepository
    {
        AppDbContext _db;
        IConfiguration _config;
        public AuthServiceRepository(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
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

        public bool CreateUser(SignUpModel user)
        {
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                User model = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber
                };
                _db.Users.Add(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserModel ValidateUser(string Email, string Password)
        {
            User user = _db.Users.Include(u=>u.Roles).Where(x => x.Email == Email).FirstOrDefault();
            if (user != null)
            {
                bool isVerified = BCrypt.Net.BCrypt.Verify(Password, user.Password);
                if (isVerified)
                {
                    UserModel model = new UserModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = user.Roles.Select(x => x.Name).ToArray()
                    };
                    model.Token = GenerateJSONWebToken(model);
                    return model;
                }
            }
            return null;
        }
    }
}
