using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Index"); // Return partial view for AJAX requests
            }

            return View("Index");

        }


    }
}
