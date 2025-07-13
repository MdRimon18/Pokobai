using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class Language
    {
       
            public int LanguageId { get; set; }
            public Guid? LanguageKey { get; set; }
            [Required(ErrorMessage = "Language Name is required")]
            [StringLength(100, ErrorMessage = "Language Name must not exceed 50 characters")]
            public string LanguageName { get; set; }
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
