using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
