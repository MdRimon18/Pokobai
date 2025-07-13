using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Domain.Services;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Doctor
{
    public class DoctorPrescriptionController : Controller
    {
        private readonly InvoiceService _invoiceService;
        private readonly InvoiceItemService _invoiceItemService;
        private readonly ProductService _productService;
        private readonly NotificationByService _notificationByService;
        private readonly InvoiceTypeService _invoiceTypeService;
        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductSubCategoryService _productSubCategoryService;
        private readonly PaymentTypesService _paymentTypesService;
        private readonly CustomerService _customerService;
        private readonly ProductSerialNumbersService _productSerialNumbersService;

        public DoctorPrescriptionController(
            InvoiceService invoiceService,
            InvoiceItemService invoiceItemService,
            ProductService productService,
            NotificationByService notificationByService,
            InvoiceTypeService invoiceTypeService,
            ProductCategoryService productCategoryService,
            ProductSubCategoryService productSubCategoryService,
            PaymentTypesService paymentTypesService,
            CustomerService customerService,
            ProductSerialNumbersService productSerialNumbersService)
        {
            _invoiceService = invoiceService;
            _invoiceItemService = invoiceItemService;
            _productService = productService;
            _notificationByService = notificationByService;
            _invoiceTypeService = invoiceTypeService;
            _productCategoryService = productCategoryService;
            _productSubCategoryService = productSubCategoryService;
            _paymentTypesService = paymentTypesService;
            _customerService = customerService;
            _productSerialNumbersService = productSerialNumbersService;
        }

        public async Task<IActionResult> Create(bool isPartial = false)
        {
            var invoiceItems = new List<InvoiceItems>
{
    new InvoiceItems
    {
        InvoiceItemId = 1,
        InvoiceId = 101,
        ProductId = 501,
        Quantity = 2,
        BuyingPrice = 150.00m,
        SellingPrice = 200.00m,
        TotalPrice = 400.00m,
        VatPercentg = 5.00m,
        VatAmount = 20.00m,
        DiscountPercentg = 10.00m,
        DiscountAmount = 40.00m,
        ExpirationDate = DateTime.Now.AddMonths(6),
        PromoOrCuppnAppliedId = null,
        ProductImage = "image1.jpg",
        CategoryName = "Paracetamol",
        ProductName = "500mg, Twice Daily",
        SubCtgName = "Audio",
        Unit = "Piece",
        LastModifyDate = DateTime.Now,
        LastModifyBy = 1001,
        DeletedDate = null,
        DeletedBy = null,
        Status = "Active"
    },
    new InvoiceItems
    {
        InvoiceItemId = 2,
        InvoiceId = 102,
        ProductId = 502,
        Quantity = 1,
        BuyingPrice = 300.00m,
        SellingPrice = 450.00m,
        TotalPrice = 450.00m,
        VatPercentg = 5.00m,
        VatAmount = 22.50m,
        DiscountPercentg = 15.00m,
        DiscountAmount = 67.50m,
        ExpirationDate = DateTime.Now.AddMonths(12),
        PromoOrCuppnAppliedId = 201,
        ProductImage = "image2.jpg",
        CategoryName = "Amoxicillin",
        ProductName = "250mg, Thrice Daily",
        SubCtgName = "Cleaning",
        Unit = "Piece",
        LastModifyDate = DateTime.Now,
        LastModifyBy = 1002,
        DeletedDate = null,
        DeletedBy = null,
        Status = "Active"
    },
    new InvoiceItems
    {
        InvoiceItemId = 3,
        InvoiceId = 103,
        ProductId = 503,
        Quantity = 3,
        BuyingPrice = 50.00m,
        SellingPrice = 80.00m,
        TotalPrice = 240.00m,
        VatPercentg = 8.00m,
        VatAmount = 19.20m,
        DiscountPercentg = 5.00m,
        DiscountAmount = 12.00m,
        ExpirationDate = DateTime.Now.AddMonths(3),
        PromoOrCuppnAppliedId = 202,
        ProductImage = "image3.jpg",
        CategoryName = "Ibuprofen",
        ProductName = "400mg, As Needed",
        SubCtgName = "Cooking",
        Unit = "Bottle",
        LastModifyDate = DateTime.Now,
        LastModifyBy = 1003,
        DeletedDate = null,
        DeletedBy = null,
        Status = "Active"
    }
};
            var model = new InvoiceViewModel
            {
                InvoiceTypeList = (await _invoiceTypeService.Get(null, null, null, null, 1, 1000)).ToList(),
                NotificationByList = (await _notificationByService.Get(null)).ToList(),
                ProductCategoryList = (await _productCategoryService.Get(null, null, null, null, 1, 1000)).ToList(),
                ProductSubCategoryList = (await _productSubCategoryService.Get(null, null, null, null, null, 1, 1000)).ToList(),
                PaymentTypesList = (await _paymentTypesService.Get(null, null, null, null, 1, 1000)).ToList(),
                Products = (await _productService.Get(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 1000)).ToList(),
                CustomersList = (await _customerService.Get(null, null, null, null, null, null, null, 1, 1000)).ToList(),
                SerialNumbers = (await _productSerialNumbersService.Get(null, null, null, null, null, null, null, null, null, null, null, 1, 5000)).ToList()
            };

            // Set default values for other model properties if needed
            model.FilteredItemsOffCanva = model.Products;

            model.ItemsList = invoiceItems;
            if (isPartial)
            {
                return PartialView("Create", model);
            }


            return View("Create", model);

        }
        public IActionResult Index(bool isPartial = false)
        {

            if (isPartial)
            {
                return PartialView("Index");
            }

            return View();

        }

    }
}
