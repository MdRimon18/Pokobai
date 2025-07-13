using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Settings
{
    public class UserPhoneNumbers:BaseEntity
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
