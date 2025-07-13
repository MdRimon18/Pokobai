using Domain.CommonServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RequestModel
{
    public class UserRequestModel
    {

        public long UserId { get; set; }
        [Required(ErrorMessage = "User Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        public string PhoneNo { get; set; }
        public string? Email { get; set; }
        public long? RoleId { get; set; } = SelectedUserRole.CustomerRoleId;
        public long CompanyId { get; set; }
        public long? BranchId { get; set; }
        public long? CountryId { get; set; } = 20012;

        public int? MembershipId { get; set; }
        public bool IsAbleToLogin { get; set; } = true;
        public string? ImgLink { get; set; }
 
    }
}
