using AuthService.Models;
using DAL.Database.Entities;

namespace AuthService.Services.Interfaces
{
    public interface IAuthenticationService
    {
        IEnumerable<UserModel> GetUsers();
        bool CreateUser(User model, string Role);
        UserModel ValidateUser(LoginModel model);
    }
}
