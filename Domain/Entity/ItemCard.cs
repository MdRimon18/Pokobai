using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    [Table("ItemCart")]
    public class ItemCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        [Required]
        public long ProductId { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string Sku { get; set; }

        public string? BrowserId { get; set; }
        public long? CustomerId { get; set; }

        [Required]
        public int Quantity { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Vat { get; set; } = 0;

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastModifyDate { get; set; }

        [NotMapped]
        public string? DetailsUrl { get; set; } 
        [NotMapped]
        public string? ImageUrl { get; set; }
        [NotMapped]
        public string? ProductName { get; set; }
        [NotMapped]
        public string? ProdCtgName { get; set; }
        [NotMapped]
        public string? ProdSubCtgName { get; set; }
        [NotMapped]
        public string? UnitName { get; set; }
        [NotMapped]
        public decimal? BuyingPrice { get; set; }
         [NotMapped]
        public decimal? SellingPrice { get; set; }
        [NotMapped]
        public long? ProductVariantId { get; set; }
    }
}
