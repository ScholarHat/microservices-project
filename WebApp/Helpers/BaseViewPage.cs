using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using System.Security.Claims;
using WebApp.Models;

namespace WebApp.Helpers
{
    public abstract class BaseViewPage<TModel> : RazorPage<TModel>
    {
        public UserViewModel CurrentUser
        {
            get
            {
                if (User.Claims.Count() > 0)
                {
                    string userData = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData).Value;
                    var user = JsonConvert.DeserializeObject<UserViewModel>(userData);
                    return user;
                }
                return null;
            }
        }
    }
}
