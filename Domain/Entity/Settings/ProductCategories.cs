using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class ProductCategories : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProdCtgId { get; set; }
        public Guid? ProdCtgKey { get; set; }
        public long? BranchId { get; set; }
        [Required(ErrorMessage = "Product Category Name is required")]
        [StringLength(100, ErrorMessage = "ProductCategories Name must not exceed 100 characters")]
        public string ProdCtgName { get; set; }
        [StringLength(250)]
        public string? ImageUrl { get; set; }
     
        public int total_row { get; set; }  
    }
}

