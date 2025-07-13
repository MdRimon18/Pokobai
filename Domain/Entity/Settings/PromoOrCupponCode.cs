using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
	public class PromoOrCupponCode
	{
		public long PromoOrCuppnId { get; set; }
		public Guid? PromoOrCuppnKey { get; set; }
        [Required(ErrorMessage = "Promo Or Cuppon Name is required")]
        [StringLength(100, ErrorMessage = "Promo Or Cuppon Name must not exceed 100 characters")]
        public string PromoOrCuppnName { get; set; }
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Discount is required")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Valid From is required")]
        public DateTime? ValidFrom { get; set; }
        [Required(ErrorMessage = "Valid To is required")]
        public DateTime? ValidTo { get; set; }
		public int? MaxUses { get; set; }
		public int? RemainingUses { get; set; }
		public string Description { get; set; }
		public DateTime? EntryDateTime { get; set; }
		public long? EntryBy { get; set; }
		public DateTime? LastModifyDate { get; set; }
		public long? LastModifyBy { get; set; }
		public DateTime? DeletedDate { get; set; }
		public long? DeletedBy { get; set; }
		public string Status { get; set; }
        public int total_row { get; set; }
    }
}

