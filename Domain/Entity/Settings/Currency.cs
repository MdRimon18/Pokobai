using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class Currency
    {
        public long CurrencyId { get; set; }
        public Guid? CurrencyKey { get; set; }
       
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Currency Name is required")]
        [StringLength(100, ErrorMessage = "Currency Name must not exceed 50 characters")]
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Currency Short Name is required")]
        public string CurrencyShortName { get; set; }

        [Required(ErrorMessage = "Symbol is required")]
        public string Symbol { get; set; }

        [Required(ErrorMessage = "Exchange Rate is required")]
        public decimal ExchangeRate { get; set; }
        public DateTime EntryDateTime { get; set; }
        public long EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        public int total_row { get; set; }
    }
}

