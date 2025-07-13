using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Settings
{
    public class UserAddressBook:BaseEntity
    {
        [Key]
        public int AddressID { get; set; } // Primary key

        [Required(ErrorMessage = "User ID is required.")]
        public long UserID { get; set; } // Foreign key to the User table

       // [Required(ErrorMessage = "Address type is required.")]
        [StringLength(50, ErrorMessage = "Address type cannot exceed 50 characters.")]
        public string? AddressType { get; set; } // e.g., Home, Office, Billing, Shipping

        [Required(ErrorMessage = "Address line 1 is required.")]
        [StringLength(255, ErrorMessage = "Address line 1 cannot exceed 255 characters.")]
        public string Address { get; set; } // Primary address line
      

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string? City { get; set; } // City

        //[Required(ErrorMessage = "State is required.")]
        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        public string? State { get; set; } // State or province

        //[Required(ErrorMessage = "Postal code is required.")]
        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
        public string? PostalCode { get; set; } // Postal or ZIP code

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public string Country { get; set; } // Country
        public string? PhoneNumber { get; set; }
        public bool IsDefault { get; set; } = false; // Indicates if this is the default address


    }
}
