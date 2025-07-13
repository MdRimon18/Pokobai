using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    public class ProductSerialNumbersController : Controller
    {
        private readonly ProductSerialNumbersService _productSerialNumbersService;

        public ProductSerialNumbersController(ProductSerialNumbersService productSerialNumbersService)
        {
            _productSerialNumbersService = productSerialNumbersService;
        }

        public async Task<IActionResult> Index(bool isPartial = false)
        {
            if (isPartial)
            {
                return PartialView("Index");
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult CreateOrEdit(long? id)
        {
            if (id == null)
            {
                return View(new ProductSerialNumbers());
            }

            var productSerialNumbers = _productSerialNumbersService.GetById(id.Value).Result;
            if (productSerialNumbers == null)
            {
                return NotFound();
            }

            return View(productSerialNumbers);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate(ProductSerialNumbers productSerialNumbers)
        {
            if (ModelState.IsValid)
            {
                if (productSerialNumbers.ProdSerialNmbrId == 0)
                {
                    productSerialNumbers.EntryDateTime = DateTime.Now;
                    productSerialNumbers.EntryBy = 1; // Replace with actual user ID
                    await _productSerialNumbersService.SaveOrUpdate(productSerialNumbers);
                }
                else
                {
                    productSerialNumbers.LastModifyDate = DateTime.Now;
                    productSerialNumbers.LastModifyBy = 1; // Replace with actual user ID
                    await _productSerialNumbersService.SaveOrUpdate(productSerialNumbers);
                }

                return RedirectToAction("Index"); // Redirect to a list page or another appropriate page
            }

            return View("Edit", productSerialNumbers);
        }
    }
}
