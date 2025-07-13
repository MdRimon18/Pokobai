
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class CountryV2
    {
        public long CountryId { get; set; }
        public Guid? CountryKey { get; set; }
       
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Country Name is required")]
        [StringLength(100, ErrorMessage = "Country Name must not exceed 50 characters")]
        public string CountryName { get; set; }
        public string CntryShortName { get; set; }
        [Required(ErrorMessage = "Country Code is required")]
       
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Capital is required")]
        
        public string Capital { get; set; }
        
       
        public int CurrencyId { get; set; }
        public decimal? CurrentArea { get; set; }
        public decimal? Population { get; set; }
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
