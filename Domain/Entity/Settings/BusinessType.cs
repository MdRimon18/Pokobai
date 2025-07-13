using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Settings
{
    public class BusinessType
    {
        public long BusinessTypeId { get; set; }
        public Guid? BusinessTypeKey { get; set; }
        public long? LanguageId { get; set; }

        [Required(ErrorMessage = "Business Type Name is required")]
        [StringLength(100, ErrorMessage = "Business Type Name must not exceed 100 characters")]
        public string BusinessTypeName { get; set; }
        public DateTime EntryDateTime { get; set; }
        public long EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string? Status { get; set; }
        public int total_row { get; set; }
    }
}
