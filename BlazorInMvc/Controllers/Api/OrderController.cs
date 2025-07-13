using Azure.Core;
using Domain.CommonServices;
using Domain.Entity;
using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Domain.Services;
using Domain.Services.Inventory;
using Domain.Services.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;

namespace BlazorInMvc.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ItemCardService _itemCardService;
        private readonly InvoiceItemService _invoiceItemService;
        private readonly InvoiceService _invoiceService;
        public OrderController(ItemCardService itemCardService, InvoiceService invoiceService, InvoiceItemService invoiceItemService)
        {
            _itemCardService = itemCardService;
            _invoiceService = invoiceService;
            _invoiceItemService = invoiceItemService;
        }
        [HttpGet("Place")]
        public async Task<IActionResult> Place(long companyId, int addressBookId, long customerId)
        {
            var itemCartList = (await _itemCardService.GetItemCartAsync(null, null, customerId, null, null)).ToList();

            if (!itemCartList.Any())
                return BadRequest("No items in cart to place an order.");

            var invoiceItems = itemCartList.Select(item => new InvoiceItems
            {
                ProductImage = item.ImageUrl,
                ProductName = item.ProductName,
                CategoryName = item.ProdCtgName,
                SubCtgName = item.ProdSubCtgName,
                Unit = item.UnitName,
                InvoiceId = 0,
                ProductId = item.ProductId,
                Quantity = (int)item.Quantity,
                SellingPrice = item.Price,
                BuyingPrice = item.BuyingPrice ?? 0,
                DiscountPercentg = 0,
                RowIndex = item.CartId,
                Status = "Active",
                ProductVariantId = item.ProductVariantId ?? 0
            }).ToList();

            var totalQuantity = invoiceItems.Sum(x => x.Quantity);
            var totalAmount = invoiceItems.Sum(x => x.SellingPrice * x.Quantity);
            var totalVat = 0M;
            var totalDiscount = invoiceItems.Sum(x => ((x.SellingPrice * x.DiscountPercentg) / 100M) * x.Quantity);
            var totalAddiDiscount = 0M;
            var totalPayable = totalAmount - totalDiscount - totalAddiDiscount;
            var receiveAmount = totalPayable;
            var dueAmount = totalPayable - receiveAmount;

            var invoiceSummary = new
            {
                TotalQuantity = totalQuantity,
                TotalAmount = totalAmount,
                TotalVat = totalVat,
                TotalDiscount = totalDiscount,
                TotalAddiDiscount = totalAddiDiscount,
                TotalPayable = totalPayable,
                RecieveAmount = receiveAmount,
                DueAmount = dueAmount
            };

            var invoice = new Invoice
            {
                InvoiceId = 0,
                CompanyId = companyId,
                BranchId = 0,
                InvoiceNumber = "E-1",
                CustomerID = customerId,
                InvoiceDateTime = DateTime.UtcNow,
                InvoiceTypeId = 10002,
                NotificationById = 1,
                SalesByName = "",
                Notes = "",
                PaymentTypeId = 10002,
                TotalQnty = (int)invoiceSummary.TotalQuantity,
                TotalAmount = (decimal)invoiceSummary.TotalAmount,
                TotalVat = (decimal)invoiceSummary.TotalVat,
                TotalDiscount = (decimal)invoiceSummary.TotalDiscount,
                TotalAddiDiscount = (decimal)invoiceSummary.TotalAddiDiscount,
                TotalPayable = (decimal)invoiceSummary.TotalPayable,
                RecieveAmount = (decimal)invoiceSummary.RecieveAmount,
                DueAmount = (decimal)invoiceSummary.DueAmount,
                Status = "Active",
                EntryDateTime = DateTime.UtcNow,
                EntryBy = customerId,
                total_row = invoiceItems.Count
            };
            long newInsertedInvoiceId = await _invoiceService.SaveOrUpdate(invoice);
            if (newInsertedInvoiceId == 0)
            {
                return BadRequest("Failed to save invoice.");
            }
            foreach (var invoiceItem in invoiceItems)
            {
                invoiceItem.InvoiceId = newInsertedInvoiceId;
                var itemId = await _invoiceItemService.SaveOrUpdate(invoiceItem);
                if (itemId == 0)
                {
                    return BadRequest($"Failed to save invoice item for ProductId {invoiceItem.ProductId}.");
                }
                invoiceItem.InvoiceItemId = itemId;
            }
            // TODO: Save invoice and invoiceItems to DB here if needed
            // await _invoiceService.SaveInvoice(invoice, invoiceItems);

            return Ok(new
            {
                Order = new { OrderId = 1 }, // Replace with real saved ID
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true
            });
        }

    }
}
