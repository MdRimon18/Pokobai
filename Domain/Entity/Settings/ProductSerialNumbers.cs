namespace Domain.Entity.Settings
{
    public class ProductSerialNumbers:BaseEntity
    {
        public long ProdSerialNmbrId { get; set; }
        public Guid? ProdSerialNmbrKey { get; set; }
        public long ProductId { get; set; }
        public string SerialNumber { get; set; }
        public long Rate { get; set; }
        public DateTime? Date { get; set; }
        public long? TagSupplierId { get; set; }
        public string Remarks { get; set; }
        public string? SerialStatus { get; set; } = "Not Sale";
        public string? SupplierName { get; set; }    
        public string? SupplierOrgName { get; set; } 
        //public DateTime? EntryDateTime { get; set; }
        //public long? EntryBy { get; set; }
        //public DateTime? LastModifyDate { get; set; }
        //public long? LastModifyBy { get; set; }
        //public DateTime? DeletedDate { get; set; }
        //public long? DeletedBy { get; set; }
        //public string? Status { get; set; }
        //public int total_row { get; set; }
    }
}
