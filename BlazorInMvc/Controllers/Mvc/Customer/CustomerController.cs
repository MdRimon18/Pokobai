using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Customer
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly CountryServiceV2 _countryServiceV2;

        public CustomerController(CustomerService customerService, CountryServiceV2 countryServiceV2)
        {
            _customerService = customerService;
            _countryServiceV2 = countryServiceV2;
        }
        [HttpGet]
        public async Task<IActionResult> Index()//bool isPartial = false, int page = 1, int pageSize = 10, string sortField = "customerName", string sortOrder = "asc")
        {
            //var customers = (await _customerService.Get(null, null, null, null, null, null, null, page, pageSize)).ToList();
            //if (customers.Count == 0)
            //{
            //    if (isPartial)
            //    {
            //        return PartialView("Index", customers);
            //    }

            //    else return View(customers);
            //}

            //var totalPages = (int)Math.Ceiling((double)customers[0].total_row / pageSize);

            //switch (sortField.ToLower())
            //{
            //    case "customername":
            //        customers = sortOrder == "asc" ? customers.OrderBy(c => c.CustomerName).ToList() : customers.OrderByDescending(c => c.CustomerName).ToList();
            //        break;
            //    case "name":
            //        customers = sortOrder == "asc" ? customers.OrderBy(c => c.CustomerName).ToList() : customers.OrderByDescending(c => c.CustomerName).ToList();
            //        break;
            //    case "email":
            //        customers = sortOrder == "asc" ? customers.OrderBy(c => c.Email).ToList() : customers.OrderByDescending(c => c.Email).ToList();
            //        break;
            //    case "mobileno":
            //        customers = sortOrder == "asc" ? customers.OrderBy(c => c.MobileNo).ToList() : customers.OrderByDescending(c => c.MobileNo).ToList();
            //        break;
            //    // Add other sortable fields as needed
            //    default:
            //        customers = customers.OrderBy(c => c.CustomerName).ToList(); // Default sorting
            //        break;
            //}

            //ViewBag.CurrentPage = page;
            //ViewBag.TotalPages = totalPages;
            //ViewBag.TotalRecords = customers[0].total_row;
            //ViewBag.PageSize = pageSize;
            //ViewBag.SortField = sortField;
            //ViewBag.SortOrder = sortOrder;

            //if (isPartial)    
            //{
            //    return PartialView("Index", customers);
            //}

            //else return View(customers);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Index"); // Return partial view for AJAX requests
            }

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> IndexV2(bool isPartial = false, int page = 1, int pageSize = 10, string sortField = "customerName", string sortOrder = "asc")
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

            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Index", customers); // Return partial view for AJAX requests
            }

            return View("Index",customers);

        }
        [HttpGet]
        public async Task<IActionResult> Create(Guid? key)
        {
            var model = new User();
            //ViewBag.SubmitButtonText = key.HasValue ? "Update" : "Create";
            //ViewBag.CountryList = (await _countryServiceV2.Get(null, null, null, null, null, null, null, null, null, null, 1, 1000)).ToList();
            model.RoleId = 10003; // Assuming 10003 is the role ID for customers
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Create", model); // Return partial view for AJAX requests
            }

            return View("Create", model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(Customers customer, IFormFile CustImgLink)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (CustImgLink != null)
        //        {
        //            // Handle file upload
        //            var filePath = Path.Combine("wwwroot/assets/CustomerImage", CustImgLink.FileName);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await CustImgLink.CopyToAsync(stream);
        //            }
        //            customer.CustImgLink = $"/assets/CustomerImage/{CustImgLink.FileName}";
        //        }

        //        if (customer.CustomerId == 0)
        //        {
        //            customer.EntryDateTime = DateTime.Now;
        //            customer.EntryBy = 1; // Replace with actual user ID
        //            customer.BranchId = 1; // Replace with actual branch ID
        //            await _customerService.SaveOrUpdate(customer);
        //        }
        //        else
        //        {
        //            customer.LastModifyDate = DateTime.Now;
        //            customer.LastModifyBy = 1; // Replace with actual user ID
        //            await _customerService.SaveOrUpdate(customer);
        //        }

        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CountryList = (await _countryServiceV2.Get(null, null, null, null, null, null, null, null, null, null, 1, 1000)).ToList();
        //    ViewBag.SubmitButtonText = customer.CustomerId == 0 ? "Create" : "Update";
        //    return View("Create", customer);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var customers = await _customerService.Get(null, null, null, null, null, null, null, 1, 10);
        //    return View(customers);
        //}
    }
}
