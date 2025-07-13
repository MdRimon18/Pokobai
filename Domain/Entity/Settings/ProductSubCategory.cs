using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class ProductSubCategory:BaseEntity
    {
        public long ProdSubCtgId { get; set; }
        public Guid? ProdSubCtgKey { get; set; }
        public long BranchId { get; set; }
        [Required(ErrorMessage = "Product Category Name is required")]
        [DisplayName("Product Category Name")]
        public long? ProdCtgId { get; set; } = null;
        [Required(ErrorMessage = "Product Sub Category Name is required")]
        [DisplayName("Product Sub Category Name")]
        public string ProdSubCtgName { get; set; }
        //public DateTime? EntryDateTime { get; set; }
        //public long? EntryBy { get; set; }
        //public DateTime? LastModifyDate { get; set; }
        //public long? LastModifyBy { get; set; }
        //public DateTime? DeletedDate { get; set; }
        //public long? DeletedBy { get; set; }
        //public string Status { get; set; }
        public int total_row { get; set; }
       public string? ProdCtgName { get; set; }
        [NotMapped]
        public List<ProductCategories> ProductCategory_list { get; set; } = new List<ProductCategories>();
    }
}
