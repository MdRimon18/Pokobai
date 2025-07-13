using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlazorInMvc.Controllers.Mvc.Supplier
{
    public class SupplierController : Controller
    {
        private readonly SupplierService _supplierService;
        private readonly CountryServiceV2 _countryServiceV2;
        private readonly BusinessTypesService _businessTypesService;
        private readonly CustomerService _customerService;

        public SupplierController(SupplierService supplierService, 
            CountryServiceV2 countryServiceV2,
            BusinessTypesService businessTypesService,
            CustomerService customerService)

        {
            _supplierService = supplierService;
            _countryServiceV2 = countryServiceV2;
            _businessTypesService = businessTypesService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? key)
        {
            var model = new Suppliers();
            ViewBag.SubmitButtonText = key.HasValue ? "Update" : "Create";
            ViewBag.CountryList = (await _countryServiceV2.Get(null, null, null, null, null, null, null, null, null, null, 1, 1000)).ToList();
            ViewBag.BusinessTypesList = (await _businessTypesService.Get(null, null, null, null, 1, 1000)).ToList();

            if (key.HasValue)
            {
                model = await _supplierService.GetByKey(key.ToString());
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate(Suppliers supplier, IFormFile SupplrImgLink)
        {
            if (ModelState.IsValid)
            {
                if (SupplrImgLink != null)
                {
                    // Handle file upload
                    var filePath = Path.Combine("wwwroot/assets/SupplierImage", SupplrImgLink.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await SupplrImgLink.CopyToAsync(stream);
                    }
                    supplier.SupplrImgLink = $"/assets/SupplierImage/{SupplrImgLink.FileName}";
                }

                if (supplier.SupplierId == 0)
                {
                    supplier.EntryDateTime = DateTime.Now;
                    supplier.EntryBy = 1; // Replace with actual user ID
                    supplier.BranchId = 1; // Replace with actual branch ID
                    await _supplierService.SaveOrUpdate(supplier);
                }
                else
                {
                    supplier.LastModifyDate = DateTime.Now;
                    supplier.LastModifyBy = 1; // Replace with actual user ID
                    await _supplierService.SaveOrUpdate(supplier);
                }

                return RedirectToAction("Index");
            }

            ViewBag.CountryList = (await _countryServiceV2.Get(null, null, null, null, null, null, null, null, null, null, 1, 1000)).ToList();
            ViewBag.BusinessTypesList = (await _businessTypesService.Get(null, null, null, null, 1, 1000)).ToList();
            ViewBag.SubmitButtonText = supplier.SupplierId == 0 ? "Create" : "Update";
            return View("Edit", supplier);
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(bool isPartial = false)
        //{
        //    var customers = (await _customerService.Get(null, null, null, null, null, null, null, 1, 10)).ToList();

        //    if (isPartial)
        //    {
        //        return PartialView("Index", customers);
        //    }
        //    return View(customers);
        //}
        //public async Task<IActionResult> Index(bool isPartial = false,int page = 1, int pageSize = 10)
        //{
        //    var customers = (await _customerService.Get(null, null, null, null, null, null, null, page, pageSize)).ToList();
        //    if (customers.Count == 0)
        //    {
        //        if (isPartial)
        //        {
        //            return PartialView("Index", customers);
        //        }
        //        else return View(customers);
        //    }

        //    var totalPages = (int)Math.Ceiling((double)customers[0].total_row / pageSize);

        //    ViewBag.CurrentPage = page;
        //    ViewBag.TotalPages = totalPages;
        //    ViewBag.TotalRecords = customers[0].total_row;
        //    if (isPartial)
        //    {
        //        return PartialView("Index", customers);
        //    }
        //    return View(customers);
        //}
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string sortField = "customerName", string sortOrder = "asc")
        {
            var customers = (await _customerService.Get(null, null, null, null, null, null, null, page, pageSize)).ToList();
            if (customers.Count == 0)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Index", customers); // Return partial view for AJAX requests
                }

                return View("Index", customers);
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
            ViewBag.PageSize=pageSize;
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Index", customers); // Return partial view for AJAX requests
            }

            return View("Index", customers);
            
        }
    }
}
