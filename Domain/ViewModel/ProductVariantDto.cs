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
        public string AttributeDetailsText { get; set; }
        public List<AttributeGroupDto> AttributeGroupDtos { get; set; }
        //public IEnumerable<object> ProductVariantAttributes { get; internal set; }
        // public List<AttributeGroupDto> AttributeGroupDto { get; set; } = new List<AttributeGroupDto>();

    }

    public class ProductVariantAttributeDto
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public List<AttributeValueDto> Values { get; set; }
    }

    public class ProductVariantsResponseDto
    {

        public ProductVariantDto? DefaultOrFirstVariants{ get; set; }

        //  public List<ProductVariantDto> ProductVariants { get; set; }
        public List<AttributeGroupDto> AttributeSets { get; set; }
      
    }

    //public class ProductVariantDto
    //{
    //    public long ProductVariantId { get; set; }
    //    public long ProductId { get; set; }
    //    public string SkuNumber { get; set; }
    //    public decimal PriceAdjustment { get; set; }
    //    public int StockQuantity { get; set; }
    //    public string ImageUrl { get; set; }
    //    public int Position { get; set; }
    //    public long SupplierId { get; set; }
    //    public string Status { get; set; }
    //    public DateTime LastModified { get; set; }
    //    public string AttributeDetailsText { get; set; }
    //    public List<AttributeGroupDto> Attributes { get; set; }
    //}

    public class AttributeGroupDto
    {
        public long AttributeId { get; set; }
        public string Attribute { get; set; }
        public List<AttributeValueDto> Values { get; set; }
    }

   
    public class AttributeValueDto
    {
        //public string Value { get; set; }
        public long ProductVariantId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string AttributeValue { get; internal set; }
        public string ImageUrl { get; set; }
        public long AttributeValueId { get; set; }
        public long ProductVariantAttributeId { get; set; }

        
    }



}
