namespace Domain.Entity.Settings
{
    public class ProductColorNdSizeAccess
    {
        public long ProdcsId { get; set; }
        public Guid ProdcsKey { get; set; }
        public long ProductId { get; set; }
        public long? ColorId { get; set; }
        public long? SizeId { get; set; }
        public decimal? Weight { get; set; }
        public long? UnitId { get; set; }
        public decimal? Qnty { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        public int total_row { get; set; }
    }
}
