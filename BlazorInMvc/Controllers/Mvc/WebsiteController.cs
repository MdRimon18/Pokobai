using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc
{
    public class WebsiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
