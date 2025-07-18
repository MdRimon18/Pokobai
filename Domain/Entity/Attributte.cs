using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ProductVariants
    {
        [Key]
        public long ProductVariantId { get; set; }

       // [ForeignKey("Products")]
        public long ProductId { get; set; }
        //public virtual Products Product { get; set; }

        [StringLength(50)]
        public string SkuNumber { get; set; } = "";

        [Required, Range(-double.MaxValue, double.MaxValue)]
        public decimal PriceAdjustment { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Position { get; set; }

        public long? SupplierId { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; } = new List<ProductVariantAttribute>();
    }

    public class ProductVariantAttribute
    {
        [Key]
        public long ProductVariantAttributeId { get; set; }

        [ForeignKey("ProductVariants")]
        public long ProductVariantId { get; set; }
        public virtual ProductVariants ProductVariant { get; set; }

        [ForeignKey("Attributte")]
        public int AttributeId { get; set; }
        public virtual Attributte Attributte { get; set; }

        [ForeignKey("AttributteValue")]
        public int AttributeValueId { get; set; }
        public virtual AttributteValue AttributeValue { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }

    public class Attributte
    {
        [Key]
        public int AttributteId { get; set; }
        public Guid AttributteKey { get; set; } = Guid.NewGuid();

        [StringLength(200)]
        public string AttributeName { get; set; }

        public string Status { get; set; }

        public DateTime LastModified { get; set; }

        public virtual ICollection<AttributteValue> AttributeValues { get; set; }
    }

    public class AttributteValue
    {
        [Key]
        public int AttributeValueId { get; set; }
        public Guid AttributeValueKey { get; set; } = Guid.NewGuid();

        [ForeignKey("Attributte")]
        public int AttributeId { get; set; }
        public virtual Attributte Attributte { get; set; }

        [StringLength(200)]
        public string AttrbtValue { get; set; }

        public string Status { get; set; }

        public DateTime LastModified { get; set; }
    }
}
