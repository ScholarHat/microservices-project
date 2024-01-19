using Microsoft.AspNetCore.Mvc;

namespace SupportApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
