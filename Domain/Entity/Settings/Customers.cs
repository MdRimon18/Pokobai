 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class Customers:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CustomerId { get; set; }
        public long? BranchId { get; set; }
        [Required(ErrorMessage = "Customer Name is Required")]
        [StringLength(100, ErrorMessage = "Customer Name cannot exceed 100 characters")]
        // [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Customer Name can only contain letters")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [StringLength(100, ErrorMessage = "Mobile Number cannot exceed 100 characters")]
        public string MobileNo { get; set; }

        // [Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
         public string? Email { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        public long? CountryId { get; set; }
        [StringLength(100)]
        public string? CountryCode { get; set; }
        [StringLength(100)]
        public string? StateName { get; set; }
        [StringLength(250)]
        public string? CustAddrssOne { get; set; }
        [StringLength(250)]
        public string? CustAddrssTwo { get; set; }
        [StringLength(100)]
        public string? Occupation { get; set; }
        [StringLength(100)]
        public string? OfficeName { get; set; }
        [StringLength(180)]
        public string? CustImgLink { get; set; }
     

        [NotMapped]
        public int total_row { get; set; } = 0;
        [NotMapped]
        public string? CountryName { get; set; }
        
    }
}
