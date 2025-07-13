using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Accounts
{
    public class AccHead
    {

        public long AccHeadId { get; set; }
        public Guid? AccHeadKey { get; set; }

        [Required(ErrorMessage = "Account Head Name is required")]
        [StringLength(100, ErrorMessage = "AccHead Name must not exceed 100 characters")]
        public string AccHeadName { get; set; }
        [Required(ErrorMessage = "Account Type is required")]
        public string AccType { get; set; }
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


