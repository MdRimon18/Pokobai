using Azure.Core;
using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorInMvc.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly InvoiceItemService _invoiceItemService;
        private readonly ProductSerialNumbersService _productSerialNumberService;
        private readonly ApplicationDbContext _context;
        private readonly CustomerService _customerService;
        private readonly InvoiceItemSerialsService _invoiceItemSerialsService;
        public InvoiceController(InvoiceService invoiceService,
            InvoiceItemService invoiceItemService,
            ProductSerialNumbersService productSerialNumberService,
            ApplicationDbContext dbContext,
            CustomerService customerService,
            InvoiceItemSerialsService invoiceItemSerialsService
            )
        {
            _invoiceService = invoiceService;
            _invoiceItemService = invoiceItemService;
            _productSerialNumberService = productSerialNumberService;
            _context = dbContext;
            _customerService = customerService;
            _invoiceItemSerialsService = invoiceItemSerialsService;
        }

        [HttpGet("GetAll")]
        
        public async Task<IActionResult> GetAll(string? search, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            
            var invoices=(await _invoiceService.Get(User.GetCompanyId(),null, null, null, null, null, null, null, null, search, page, pageSize)).ToList();
           
            

            if (invoices.Count == 0)
            {
                return Ok(new
                {
                    items = invoices,
                    currentPage = page,
                    totalPages = 0,
                    totalRecord = 0
                });
            }
            var totalRecord = invoices[0].total_row;
            var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            return Ok(new
            {
                items = invoices,
                currentPage = page,
                totalPages,
                totalRecord
            });
        }

        [HttpPost("save-items")]
        public async Task<IActionResult> SaveItems([FromBody] SaveInvoiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Begin transaction
                using var transaction = await _context.Database.BeginTransactionAsync();

                // 1. Map and save Invoice
                if (!long.TryParse(request.InvoiceSummary.CustomerId, out var customerId))
                    return BadRequest("Invalid CustomerId format.");
                if (!DateTime.TryParse(request.InvoiceSummary.InvoiceDate, out var invoiceDate))
                    return BadRequest("Invalid InvoiceDate format.");
                long? notificationById = long.TryParse(request.InvoiceSummary.NotificationById, out var parsedNotificationById) ? parsedNotificationById : null;
                long? paymentTypeId = long.TryParse(request.InvoiceSummary.PaymentTypeId, out var parsedPaymentTypeId) ? parsedPaymentTypeId : null;

                var invoice = new Invoice
                {
                    CompanyId=User.GetCompanyId(),
                    InvoiceId=request.InvoiceSummary.InvoiceId,
                    BranchId = CompanyInfo.BranchId,
                    InvoiceNumber =request.InvoiceSummary.InvoiceNumber,
                    CustomerID = customerId,
                    InvoiceDateTime = invoiceDate,
                    InvoiceTypeId = 1, // Sales Point Set based on business logic if needed
                    NotificationById = notificationById,
                    SalesByName = request.InvoiceSummary.Seller,
                    Notes = request.InvoiceSummary.Notes,
                    PaymentTypeId = paymentTypeId,
                    TotalQnty = (int)request.InvoiceSummary.TotalQuantity,
                    TotalAmount = (decimal)request.InvoiceSummary.TotalAmount,
                    TotalVat = (decimal)request.InvoiceSummary.TotalVat,
                    TotalDiscount = (decimal)request.InvoiceSummary.TotalDiscount,
                    TotalAddiDiscount = (decimal)request.InvoiceSummary.TotalAddiDiscount,
                    TotalPayable = (decimal)request.InvoiceSummary.TotalPayable,
                    RecieveAmount = (decimal)request.InvoiceSummary.RecieveAmount,
                    DueAmount = (decimal)request.InvoiceSummary.DueAmount,
                    OrderStageId=request.InvoiceSummary.OrderStageId,
                    Status = "Active",
                    EntryDateTime = DateTime.UtcNow,
                    EntryBy = UserInfo.UserId,
                    total_row = request.Items.Count
                };

                long newInsertedInvoiceId = await _invoiceService.SaveOrUpdate(invoice);
                if (newInsertedInvoiceId == 0)
                {
                    return BadRequest("Failed to save invoice.");
                }

                // 2. Map and save InvoiceItems using a loop
                var invoiceItems = request.Items
                    .Select(item => new InvoiceItems
                {
                    ProductImage=item.ProductImage,
                    ProductName=item.ProductName,
                    CategoryName=item.CategoryName,
                    SubCtgName=item.SubCtgName,
                    Unit=item.Unit,

                    InvoiceId = newInsertedInvoiceId,
                    ProductId = item.ProductId,
                    InvoiceItemId=item.InvoiceItemId,
                    Quantity = (int)item.Quantity,
                    SellingPrice = (decimal)item.SellingPrice,
                    BuyingPrice=item.BuyingPrice,
                    DiscountPercentg = (decimal)item.DiscountPercentg,
                    RowIndex = item.RowIndex,
                    Status = "Active",
                    SelectedSerialNumbers = item.Serials?.Select(serial => new InvoiceItemSerials
                    {
                        SerialNumber = serial.SerialNumber,
                        ProdSerialNmbrId = long.TryParse(serial.ProdSerialNmbrId, out var prodSerialId) ? prodSerialId : 0,
                        InvoiceId= newInsertedInvoiceId,
                        Rate=serial.Rate
                        //SupplierOrgName = serial.SupplierOrgName,
                        //SerialStatus = "Sale"
                    }).ToList() ?? new List<InvoiceItemSerials>(),
                    ProductVariantId=item.ProductVariantId
                    }).ToList();

                foreach (var invoiceItem in invoiceItems)
                {
                    var itemId = await _invoiceItemService.SaveOrUpdate(invoiceItem);
                    if (itemId == 0)
                    {
                        return BadRequest($"Failed to save invoice item for ProductId {invoiceItem.ProductId}.");
                    }
                    invoiceItem.InvoiceItemId = itemId;

                    if(invoiceItem.SelectedSerialNumbers != null && invoiceItem.SelectedSerialNumbers.Any())
                    {
                        foreach (var serial in invoiceItem.SelectedSerialNumbers)
                        {
                           serial.InvoiceItemId = itemId;
                          await  _invoiceItemSerialsService.AddAsync(serial);
                            //  var serialId = await _productSerialNumberService.SaveOrUpdateAsync(serial);

                        }
                    }
                }

                // 3. Save ProductSerialNumbers using a loop
                //var allSerialNumbers = invoiceItems
                //    .SelectMany(x => x.SelectedSerialNumbers)
                //    .ToList();
                //foreach (var serialNumber in allSerialNumbers)
                //{
                //    var serialId = await _productSerialNumberService.SaveOrUpdateAsync(serialNumber);
                //    if (serialId == 0)
                //    {
                //        throw new InvalidOperationException($"Failed to save serial number {serialNumber.SerialNumber}.");
                //    }
                //}

                // Commit transaction
                await transaction.CommitAsync();
                var invoiceObj = await _invoiceService.GetById(User.GetCompanyId(),newInsertedInvoiceId);
                var itemViewList = new List<InvoiceItemViewModel>();
                foreach (var item in invoiceItems)
                {
                    // Retrieve serial numbers asynchronously
                    var serialNumbers = await _invoiceItemSerialsService.GetByInvoiceItemId(item.InvoiceItemId);

                    var viewModel = new InvoiceItemViewModel
                    {
                        InvoiceItemId = item.InvoiceItemId,
                        InvoiceId = newInsertedInvoiceId,
                        ProductId = item.ProductId,
                        Quantity = (int)item.Quantity,
                        SellingPrice = (decimal)item.SellingPrice,
                        TotalPrice = (decimal)item.SellingPrice * (int)item.Quantity, // Calculate TotalPrice (adjust if there's a specific formula)
                        VatPercent = item.VatPercentg, // Handle if VatPercentg is not provided in request.Items
                        DiscountPercentg = (decimal)item.DiscountPercentg,
                        ImageUrl = item.ProductImage,
                        ProdName = item.ProductName,
                        ProdCtgName = item.CategoryName,
                        ProdSubCtgName = item.SubCtgName,
                        UnitName = item.Unit,
                        SelectedSerialNumbers = serialNumbers?.ToList() ?? new List<InvoiceItemSerials>()
                    };

                    itemViewList.Add(viewModel);
                }
                return Ok(new
                {
                    InvoiceKey= invoiceObj?.InvoiceKey,
                    InvoiceId = newInsertedInvoiceId,
                    InvoiceItems= itemViewList,
                    Message = "Invoice Created successfully!"
                });
            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                return StatusCode(500, new { Error = "An error occurred while saving the invoice.", Details = ex.Message });
            }
        }
    }
}
