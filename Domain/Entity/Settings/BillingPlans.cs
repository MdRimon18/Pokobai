using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class BillingPlans : BaseEntity
    {
        [Key]
        public long BillingPlanId { get; set; }
        public Guid BillingPlanKey { get; set; }
        public int LanguageId { get; set; }
        [Required(ErrorMessage = "Billing Plan Name is required")]
        [DisplayName("Billing Plan Name")]
        public string BillingPlanName { get; set; }
        public int total_row { get; set; }
    }
}
