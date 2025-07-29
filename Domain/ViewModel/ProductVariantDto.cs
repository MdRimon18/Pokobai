using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class ProductVariantDto
    {
        public long ProductVariantId { get; set; }
        public long ProductId { get; set; }
        public string SkuNumber { get; set; }
        public decimal PriceAdjustment { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public int Position { get; set; }
        public long? SupplierId { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
        public List<ProductVariantAttributeDto> Attributes { get; set; }
    }

    public class ProductVariantAttributeDto
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public List<AttributeValueDto> Values { get; set; }
    }

    public class AttributeValueDto
    {
        public long ProductVariantAttributeId { get; set; }
        public int AttributeValueId { get; set; }
        public string AttributeValue { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
    }
}
