using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class CustomerReview
    {
        public long ReviewId { get; set; }
        public Guid? ReviewKey { get; set; }
        public string ReviewName { get; set; }
        [Required(ErrorMessage = "Review Email is required")]
        [StringLength(100, ErrorMessage = "Review Email exceed 150 characters")]
        public string ReviewEmail { get; set; }
        public int? StarCount { get; set; }
        public string ReviewDetails { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public int total_row { get; set; } = 0;

    }
}

