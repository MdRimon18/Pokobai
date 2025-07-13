using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Settings
{
    public class UserDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserDetailId { get; set; }
        public long UserId { get; set; }
        [StringLength(100)]
        public string? PhoneTwo { get; set; }
        [StringLength(100)]
        public string? EmailTwo { get; set; }
        [StringLength(100)]
        public string? NidNo { get; set; }
        [StringLength(100)]
        public string? PassportNo { get; set; }
        [StringLength(200, ErrorMessage = "Organization Name cannot exceed 200 characters")]
        public string? OrganizationName { get; set; }

        [StringLength(100)]
        public string? Occupation { get; set; }
        [StringLength(100)]
        public string? OfficeName { get; set; }
        public long? BusinessTypeId { get; set; }
        public string? OffDay { get; set; }
        [StringLength(180)]
        public string? OrgImgLink { get; set; }
        public int NumberOfStaff { get; set; } = 0;
        [StringLength(500)]
        public string? Note { get; set; }
    }
}
