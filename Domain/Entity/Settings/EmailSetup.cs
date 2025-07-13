using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class EmailSetup
    {
        public int EmailSetupId { get; set; }
        public Guid EmailSetupkey { get; set; }
        public long BranchId { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string FromEmail { get; set; }
        [Required(ErrorMessage = "From Name is required")]
        public string FromName { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        [Required(ErrorMessage = "Port Number is required")]
        public long? PortNumber { get; set; }
        public bool IsDefault { get; set; }
        public DateTime EntryDateTime { get; set; }
        public long EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        public int total_row { get; set; }
    }
}
