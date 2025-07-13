using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class PaymentTypes
    {
        public long PaymentTypeId { get; set; }
        public Guid PaymentTypeKey { get; set; }
        public int LanguageId { get; set; }
        [Required(ErrorMessage = "Payment Type Name is required")]
        [DisplayName("Payment Type Name")]
        public string PaymentTypesName { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string? Status { get; set; }
        public int total_row { get; set; }  
    }
}

