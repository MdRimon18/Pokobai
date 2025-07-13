using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class Suppliers:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SupplierId { get; set; }
        public long? BranchId { get; set; }
        [Required(ErrorMessage = "Supplier Name is Required")]
        [StringLength(100, ErrorMessage = "Supplier Name cannot exceed 100 characters")]
        public string SupplrName { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required")]
        [StringLength(100, ErrorMessage = "Mobile Number cannot exceed 100 characters")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
           
        public string Email { get; set; }
        [Required(ErrorMessage = "Organization Name is Required")]
        [StringLength(200, ErrorMessage = "Organization Name cannot exceed 200 characters")]
        public string? SuppOrgnznName { get; set; }
        [Required(ErrorMessage = "Country is Required")]
        public long? CountryId { get; set; }
        [StringLength(100)]
        public string? CountryCode { get; set; }

        [Required(ErrorMessage = "State Name is Required")]
        [StringLength(100, ErrorMessage = "State Name cannot exceed 100 characters")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Business type is Required")]
        public long BusinessTypeId { get; set; }
        public string? SupplrAddrssOne { get; set; }
        public string? SupplrAddrssTwo { get; set; }
        public string? DeliveryOffDay { get; set; }
        public string? SupplrImgLink { get; set; }
       

        [NotMapped]
        public int total_row { get; set; } = 0;
        [NotMapped]
        public string ImageData { get; set; }

    }
}
