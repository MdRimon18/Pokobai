using Domain.CommonServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entity.Settings
{
    public class Invoice
    {
        public long InvoiceId { get; set; }
        public Guid? InvoiceKey { get; set; }
        public long CompanyId { get; set; } = CompanyInfo.CompanyId;
        public long BranchId { get; set; }
        [Required(ErrorMessage = "Invoice Number is required")]
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public DateTime InvoiceDateTime { get; set; }= DateTime.Now;

        [Required(ErrorMessage = "Invoice Type is required")]
      
        public long? InvoiceTypeId { get; set; } =null;
        [Required(ErrorMessage = "Notification is required")]
        public long? NotificationById { get; set; } = null; 
        public string SalesByName { get; set; }
        public string Notes { get; set; }

        [Required(ErrorMessage = "PaymentType is required")]
        public long? PaymentTypeId { get; set; }=null;
        public string PaymentReference { get; set; }
        public long? ShippingMethodId { get; set; }
        public long CurrencyId { get; set; }
        public long? OrderStatusId { get; set; }
        public int TotalQnty { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public decimal TotalVat { get; set; } = 0;
        public decimal TotalDiscount { get; set; } = 0;
        // Custom setter for TotalAddiDiscount
       
        public decimal TotalAddiDiscount { get; set; } = 0;
        public decimal TotalPayable { get; set; } = 0;
        public decimal RecieveAmount { get; set; } = 0;
        public decimal DueAmount { get; set; } = 0;
        public DateTime? DuePaymentDate { get; set; }
        public long? PromoOrCupponId { get; set; }
        public long? PolicyId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? PriorityLevelId { get; set; }
        public bool? GiftOrder { get; set; }
        public string VoucherCode { get; set; }
        public string ShipmentTrackingNumber { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? DeliveryAddressId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        public int total_row { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? InvoiceTypeName { get; set; }
        [NotMapped]
        public string? DeliverAddress { get; set; }
        [NotMapped]
        public string? FormattedEntryDateTime { get; set; }
        //private void RecalculateTotalPayable()
        //{
        //    TotalPayable = TotalAmount + TotalVat - TotalDiscount - TotalAddiDiscount;
        //}



    }
}
