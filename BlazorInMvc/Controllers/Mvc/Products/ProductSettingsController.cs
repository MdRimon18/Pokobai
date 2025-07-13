using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    public class ProductSettingsController : Controller
    {
        public IActionResult Index(bool isPartial = false)
        {
            if (isPartial)
            {
                return PartialView("Index");
            }
            return View("Index");

        }
    }
}
