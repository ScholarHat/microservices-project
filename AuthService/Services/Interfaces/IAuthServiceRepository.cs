using AuthService.Model;

namespace AuthService.Services.Interfaces
{
    public interface IAuthServiceRepository
    {
        UserModel ValidateUser(string Email, string Password);
        bool CreateUser(SignUpModel user);
    }
}
