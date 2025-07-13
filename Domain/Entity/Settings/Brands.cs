using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class Brands
    {
        public long BrandId { get; set; }
        public Guid? BrandKey { get; set; }
        [Required(ErrorMessage = " Brand Name is required")]
        [StringLength(100, ErrorMessage = " Brand Name must not exceed 300 characters")]
        public string BrandName { get; set; }
        public string BrandDetails { get; set; }
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
