using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    
    public class ProductCategoryTypes
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string CategoryTypeCode { get; set; }
        public long ProductId { get; set; }
        public long CompanyId { get; set; }
    }
}
