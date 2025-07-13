namespace Domain.Entity.Settings
{
    public class ProductSpecifications:BaseEntity
    {
        public long ProdSpcfctnId { get; set; }
        public Guid? ProdSpcfctnKey { get; set; }
        public long ProductId { get; set; }
        public string HeaderName { get; set; }
        public string SpecificationName { get; set; }
        public string SpecificationDtls { get; set; }
    
        public int total_row { get; set; }
    }
}
