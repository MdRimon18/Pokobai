using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Settings
{
    public class Unit : BaseEntity
    {
        public long UnitId { get; set; }
        public Guid? UnitKey { get; set; }
        public long? BranchId { get; set; }
        [Required(ErrorMessage = "Unit Name is required")]
        [StringLength(100, ErrorMessage = "Unit Name must not exceed 100 characters")]
        public string UnitName { get; set; }
        
    }
}
