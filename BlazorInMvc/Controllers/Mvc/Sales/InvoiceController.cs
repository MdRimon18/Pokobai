using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Domain.Services;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Sales
{
    [Authorize]
    public class InvoiceController : Controller
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
        private readonly InvoiceItemSerialsService _invoiceItemSerialsService;
        private readonly OrderStageService _orderStageService;
        public InvoiceController(
            InvoiceService invoiceService,
            InvoiceItemService invoiceItemService,
            ProductService productService,
            NotificationByService notificationByService,
            InvoiceTypeService invoiceTypeService,
            ProductCategoryService productCategoryService,
            ProductSubCategoryService productSubCategoryService,
            PaymentTypesService paymentTypesService,
            CustomerService customerService,
            ProductSerialNumbersService productSerialNumbersService,
            InvoiceItemSerialsService invoiceItemSerialsService,
            OrderStageService orderStageService)
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
            _invoiceItemSerialsService = invoiceItemSerialsService;
            _orderStageService = orderStageService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        { 
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Index"); // Return partial view for AJAX requests
            }

            return View("Index");
        }

        public async Task<IActionResult> Create(Guid? key)
        {
            Invoice invoice = new Invoice();
            if (key != null && key != Guid.Empty)
            {
                  invoice = await _invoiceService.GetByKey( User.GetCompanyId(),key.ToString());
            }
           
            IEnumerable<InvoiceItems> itemsList = new List<InvoiceItems>();
            if (invoice.InvoiceId > 0)
            {
                itemsList = await _invoiceItemService.Get(null, invoice.InvoiceId, 1, 100);
            }


            var invoiceItemViewModels = new List<InvoiceItemViewModel>();

            foreach (var item in itemsList)
            {
                var serialNumbers = await _invoiceItemSerialsService.GetByInvoiceItemId(item.InvoiceItemId);

                var viewModel = new InvoiceItemViewModel
                {
                    InvoiceItemId = item.InvoiceItemId,
                    InvoiceId = item.InvoiceId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SellingPrice = item.SellingPrice,
                    TotalPrice = item.TotalPrice,
                    VatPercent = item.VatPercentg,
                    DiscountPercentg = item.DiscountPercentg,
                    ImageUrl = item.ProductImage,
                    ProdName = item.ProductName,
                    ProdCtgName = item.CategoryName,
                    ProdSubCtgName = item.SubCtgName,
                    UnitName = item.Unit,
                    SelectedSerialNumbers = serialNumbers?.ToList() ?? new List<InvoiceItemSerials>(),
                    ProductVariantId = item.ProductVariantId,
                    AttributeDetailsText=item.AttributeDetailsText
                };

                invoiceItemViewModels.Add(viewModel);
            }

           

            var model = new InvoiceViewModel
            {
                CustomerId=invoice.CustomerID,
                CustomerName=invoice.CustomerName,
                Invoice=invoice,
                InvoiceTypeList = (await _invoiceTypeService.Get(null, null, null, null, 1, 1000)).ToList(),
                NotificationByList = (await _notificationByService.Get(null)).ToList(),
                ProductCategoryList = (await _productCategoryService.Get(User.GetCompanyId(), null, null, null, null, 1, 1000)).ToList(),
                ProductSubCategoryList = (await _productSubCategoryService.Get(null, null, null, null, null, 1, 1000)).ToList(),
                PaymentTypesList = (await _paymentTypesService.Get(null, null, null, null, 1, 1000)).ToList(),
                Products = (await _productService.Get(User.GetCompanyId(), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 1000)).ToList(),
                CustomersList = (await _customerService.Get(null, null, null, null, null, null, null, 1, 1000)).ToList(),
                SerialNumbers = (await _productSerialNumbersService.Get(null, null, null, null, null, null, null, null, null, null, null, 1, 5000)).ToList(),
                OrderStages = await _orderStageService.GetAllAsync()

            };

            // Set default values for other model properties if needed
            model.FilteredItemsOffCanva = model.Products;
            model.ItemsListViewModel = invoiceItemViewModels;//for showing invoice items
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Create", model); // Return partial view for AJAX requests
            }

            return View("Create", model);   

        }
        //public IActionResult Index(bool isPartial = false)
        //{
        //    InvoiceViewModel invoiceViewModel = new InvoiceViewModel();
        //    if (isPartial)
        //    {
        //        return PartialView("Index", invoiceViewModel);
        //    }
        //    return View("Index", invoiceViewModel);

        //}

        [HttpGet]
        public async Task<bool> RemoveData(long id)
        {

            try
            {
                if (id>0)
                {
                   var model = await _invoiceService.Delete(User.GetCompanyId(), id);
                    return true;
                   
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IActionResult ShippingWithPayment()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("ShippingWithPayment"); // Return partial view for AJAX requests
            }
            return View("ShippingWithPayment");
        }
        
        public IActionResult PrintInvoice(bool isPartial = false)
        {

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("PrintInvoice"); // Return partial view for AJAX requests
            }
            return View("PrintInvoice");
           

        }
    }
}
