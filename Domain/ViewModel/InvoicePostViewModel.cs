using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
   
        public class SaveInvoiceRequest
        {
            public InvoiceSummaryDto InvoiceSummary { get; set; }
            public List<ItemDto> Items { get; set; }
        }

        public class InvoiceSummaryDto
        {
    
            public long InvoiceId { get; set; }
            public string? InvoiceNumber { get; set; }
            public string CustomerId { get; set; }
            public string? CustomerName { get; set; }
            public string? Mobile { get; set; }
            public string? CustomerEmail { get; set; }
            public string NotificationById { get; set; }
            public string InvoiceDate { get; set; }
            public string? Seller { get; set; }
            public int? OrderStageId { get; set; }
            public string PaymentTypeId { get; set; }
            public string? CategoryId { get; set; }
            public string SubCategoryId { get; set; }
            public string? ProductSearch { get; set; }
            public string? Notes { get; set; }

            public float TotalQuantity { get; set; }
            public float TotalAmount { get; set; }
            public float TotalVat { get; set; }
            public float TotalDiscount { get; set; }
            public float TotalAddiDiscount { get; set; }
            public float TotalPayable { get; set; }
            public float RecieveAmount { get; set; }
            public float DueAmount { get; set; }
            public DateTime EntryDateTime { get; set; }=DateTime.UtcNow;
            public long? EntryBy { get; set; }
        public int? DeliveryAddressId { get; set; }
    }

        public class ItemDto
        {
            public long ItemId { get; set; }
            public int RowIndex { get; set; }
            public int ProductId { get; set; }
            public long InvoiceItemId { get; set; }
            public float Quantity { get; set; }
            public float SellingPrice { get; set; }
          public decimal BuyingPrice { get; set; } = 0;
          public float DiscountPercentg { get; set; }
          public string? ProductImage { get; set; }
          public string CategoryName { get; set; }
          public string ProductName { get; set; }
          public string? SubCtgName { get; set; }
          public string? Unit { get; set; }

        public List<SerialDto> Serials { get; set; }
            public long? ProductVariantId { get; set; }
    }

        public class SerialDto
        {
            public string SerialNumber { get; set; }
            public string ProdSerialNmbrId { get; set; }
            public string? SupplierOrgName { get; set; }
            public  double Rate { get; set; }
    }
    
}
