using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class Colors : BaseEntity
    {
        public long ColorId { get; set; }
        public Guid? ColorKey { get; set; }
        public int LanguageId { get; set; }
        [Required(ErrorMessage = "Color Name is required")]
        [DisplayName("Colors Name")]
        public string ColorIdName { get; set; }
         
        
    }
}

