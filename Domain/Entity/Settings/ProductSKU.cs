namespace Domain.Entity.Settings
{
    public class ProductSKU
    {
        public long ProdSkuId { get; set; }
        public Guid? ProdSkuKey { get; set; }
        public long ProductId { get; set; }
        public string Sku { get; set; }
        public long? ColorId { get; set; }
        public long? SizeId { get; set; }
        public decimal? Weight { get; set; }
        public long? UnitId { get; set; }
        public decimal? StockQnty { get; set; }
        public long? WarehouseId { get; set; }
        public long? SupplierId { get; set; }
        public decimal? BuyingPrice { get; set; }
        public long? CurrencyId { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? LeadTimeDays { get; set; }
        public string RackNumber { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
    }
}
