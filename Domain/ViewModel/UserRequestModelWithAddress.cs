using Domain.CommonServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class UserRequestModelWithAddress
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        public string PhoneNo { get; set; }
    //    public long? RoleId { get; set; } = SelectedUserRole.CustomerRoleId;
        [Required(ErrorMessage = "CompanyId is Required")]
        public long CompanyId { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        public string? City { get; set; }
        public int AddressID { get; set; }
        public string? BrowserId { get; set; }
        public long OrderId { get; set; }

    }
    
}
