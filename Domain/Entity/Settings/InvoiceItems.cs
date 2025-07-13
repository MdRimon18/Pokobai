using Domain.Entity.Inventory;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
	public class InvoiceItems
	{
		public long InvoiceItemId { get; set; } = 0;
		public long InvoiceId { get; set; }
		public long ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal BuyingPrice { get; set; } = 0;
		public decimal SellingPrice { get; set; } = 0;
		public decimal TotalPrice { get; set; } = 0;
		public decimal VatPercentg { get; set; } = 0;
		public decimal VatAmount { get; set; } = 0;
		public decimal DiscountPercentg { get; set; } = 0;
		public decimal DiscountAmount { get; set; } = 0;
		public DateTime? ExpirationDate { get; set; }
		public long? PromoOrCuppnAppliedId { get; set; }
		public string ProductImage { get; set; }
		public string CategoryName { get; set; }
		public string ProductName { get; set; }
		public string SubCtgName { get; set; }
		public string Unit { get; set; }
		public DateTime? LastModifyDate { get; set; }
		public long? LastModifyBy { get; set; }
		public DateTime? DeletedDate { get; set; }
		public long? DeletedBy { get; set; }
		public string Status { get; set; }
        public long? ProductVariantId { get; set; }
        [NotMapped]
		public List<InvoiceItemSerials> SelectedSerialNumbers { get; set; } = new List<InvoiceItemSerials>();
		[NotMapped]
		public string ImageUrl { get; set; }
		public int RowIndex { get; set; } //add to database 
	}
}
