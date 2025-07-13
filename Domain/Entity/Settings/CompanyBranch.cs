using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class CompanyBranch
    {
        public long BranchId { get; set; }
        public Guid BranchKey { get; set; }
        public long CompanyId { get; set; }
        [Required(ErrorMessage = "Branch Name is required")]
        [StringLength(100, ErrorMessage = "Branch Name cannot exceed 150 characters")]
        public string BranchName { get; set; }
        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(100, ErrorMessage = "Mobile Number cannot exceed 100 characters")]
        public string MobileNo { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "State name is required")]
        [StringLength(100, ErrorMessage = "State name exceed 100 characters")]
        public string StateName { get; set; }
        [Required(ErrorMessage = "Adress is required")]
        public string Address { get; set; }
        public string BrnchManagerName { get; set; }
        public string ManagerMobile { get; set; }
        public string BranchImgLink { get; set; }
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
