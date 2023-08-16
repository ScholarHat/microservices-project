using Microsoft.AspNetCore.Mvc;

namespace SupportPortal.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
