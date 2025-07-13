using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Settings
{
    public class CurrencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
