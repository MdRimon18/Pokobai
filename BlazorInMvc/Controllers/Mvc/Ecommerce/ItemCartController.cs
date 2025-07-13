using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Ecommerce
{
    public class ItemCartController : Controller
    {
        public IActionResult Index(bool isPartial = false)
        {
            if (isPartial)
            {
                return PartialView("Index");
            }
            return View("Index");

        }
        public IActionResult Checkout(bool isPartial = false)
        {
            if (isPartial)
            {
                return PartialView("Checkout");
            }
            return View("Checkout");

        }
    }
}
