﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using WebApp.Models;

namespace WebApp.Areas.User.Controllers
{
    [Area("User")]
    public class BaseController : Controller
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
