using Domain.Services.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlazorInMvc.Controllers.Mvc.Ecommerce
{
    public class OrderController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly CountryServiceV2 _countryServiceV2;

        public OrderController(CustomerService customerService, CountryServiceV2 countryServiceV2)
        {
            _customerService = customerService;
            _countryServiceV2 = countryServiceV2;
        }
        [HttpGet]
        public async Task<IActionResult> Index(bool isPartial = false, int page = 1, int pageSize = 10, string sortField = "customerName", string sortOrder = "asc")
        {
            var customers = (await _customerService.Get(null, null, null, null, null, null, null, page, pageSize)).ToList();
            if (customers.Count == 0)
            {
                if (isPartial)
                {
                    return PartialView("Index", customers);
                }

                else return View(customers);
            }

            var totalPages = (int)Math.Ceiling((double)customers[0].total_row / pageSize);

            switch (sortField.ToLower())
            {
                case "customername":
                    customers = sortOrder == "asc" ? customers.OrderBy(c => c.CustomerName).ToList() : customers.OrderByDescending(c => c.CustomerName).ToList();
                    break;
                case "name":
                    customers = sortOrder == "asc" ? customers.OrderBy(c => c.CustomerName).ToList() : customers.OrderByDescending(c => c.CustomerName).ToList();
                    break;
                case "email":
                    customers = sortOrder == "asc" ? customers.OrderBy(c => c.Email).ToList() : customers.OrderByDescending(c => c.Email).ToList();
                    break;
                case "mobileno":
                    customers = sortOrder == "asc" ? customers.OrderBy(c => c.MobileNo).ToList() : customers.OrderByDescending(c => c.MobileNo).ToList();
                    break;
                // Add other sortable fields as needed
                default:
                    customers = customers.OrderBy(c => c.CustomerName).ToList(); // Default sorting
                    break;
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = customers[0].total_row;
            ViewBag.PageSize = pageSize;
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            if (isPartial)
            {
                return PartialView("Index", customers);
            }

            else return View(customers);
        }
    }
}
