using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entity.Settings
{
    public class User:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "User Phone No is Required")]
        public string PhoneNo { get; set; }
        
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "Role Name is Required")]
        public long RoleId { get; set; }
        public long? CompanyId { get; set; }
        public long? BranchId { get; set; }
        public long? CountryId { get; set; }
        [StringLength(100)]
        public string? CountryCode { get; set; }
        [StringLength(100)]
        public int? MembershipId { get; set; }
        public bool IsAbleToLogin { get; set; } = true;
        public string? ImgLink { get; set; }
     

        [NotMapped]
        public int total_row { get; set; }
        [NotMapped]
        public bool RememberMe { get; set; } = false;
        [NotMapped]
        public string? RoleName { get; set; }
        [NotMapped]
        public string? PageName { get; set; }
        [NotMapped]
        public string?Address { get; set; }
    }
}
