using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class Company
    {
        public long CompanyId { get; set; }
        public Guid CompanyKey { get; set; }
        public int LanguageId { get; set; }
        public long OwnerOrUserId { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 150 characters")]
        public string CompanyName { get; set; }
        
        [Required(ErrorMessage = "Business type is required")]
        public long BusinessTypeId { get; set; }
        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(100, ErrorMessage = "Mobile Number cannot exceed 100 characters")]
        public string CompMobileNo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? CompanyEmail { get; set; }
        [Required(ErrorMessage = "Country name is required")]
        public long CountryId { get; set; }
        [Required(ErrorMessage = "State name is required")]
        [StringLength(100, ErrorMessage = "State name exceed 100 characters")]
        public string StateName { get; set; }
        public string CompAddrss { get; set; }
        [Required(ErrorMessage = "Currency name is required")]
        public long CurrencyId { get; set; }
        [Required(ErrorMessage = "Billing plan is required")]
        public long BillingPlanId { get; set; }
        public string? WorkingDays { get;set; }
        public string? StartToEndTime { get; set; }
        public string? CompanyLogoImgLink { get; set; }
        public bool IsHasBranch { get; set; }
        public string MobileCode { get; set; }
        public string TimeSpent { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string BusinessTypeName { get; set; }
        [NotMapped]
        public int total_row { get; set; } = 0;

    }
}
