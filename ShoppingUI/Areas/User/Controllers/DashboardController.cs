using Microsoft.AspNetCore.Mvc;

namespace ShoppingUI.Areas.User.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
