using Domain.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{

    public class AttributteViewModel
    {
        public int AttributteId { get; set; }
        public Guid AttributteKey { get; set; }
        public string AttributeName { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
        public List<AttributteValueViewModel> AttributeValues { get; set; } = new();
    }

    public class AttributteValueViewModel
    {
        public int AttributeValueId { get; set; }
        public int AttributeId { get; set; }
        public string AttrbtValue { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class AttributteWithDetailsViewModel
    {
        public int AttributteId { get; set; }
        public string AttributeName { get; set; }
        public string Status { get; set; }
        public string[] AttributteDetails { get; set; }
    }

    public class AttributeSetViewModel
    {
        public int AttributeSetId { get; set; }
        public Guid AttributeSetKey { get; set; }
        public string AttributeSetName { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class AttributeSetDetailViewModel
    {
        public int AttributeSetDetailId { get; set; }
        public int AttributeSetId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
    }

    public class AttributeSetWithDetailsViewModel
    {
        public int AttributeSetId { get; set; }
        public string AttributeSetName { get; set; }
        public string Status { get; set; }
        public int[] AttributeIds { get; set; }
    }

    public class ProductVariantViewModel
    {
        public long ProductVariantId { get; set; }

        public long ProductId { get; set; }

        [StringLength(50)]
        public string SkuNumber { get; set; } = string.Empty;

        [Required]
        [Range(-double.MaxValue, double.MaxValue)]
        public decimal PriceAdjustment { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; } = 0;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Position { get; set; }

        public long? SupplierId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        // For selected attribute value IDs from UI
        public List<int> AttributeValuesId { get; set; } = new();

        // For image upload from UI
        public IFormFile? File { get; set; }

        // Optional: For displaying related attribute data in the view
        public List<ProductVariantAttribute>? ProductVariantAttributes { get; set; }
        public string AttributeDetailsText { get; set; }
        public List<AttributteValue> AllAttributeValues { get; set; } = new();
    }


    public class ProductAttributeDetailViewModel
    {
        public int ProductAttributeDetailId { get; set; }
        public int ProductAttributeId { get; set; }
        public int AttributteId { get; set; }
        public int AttributteValueId { get; set; }
        public string AttributteValue { get; set; }
        public string AttributteName { get; set; }
    }

    public class ProductAttributeWithDetailsViewModel
    {
        public int ProductAttributeId { get; set; }
        public int ProductId { get; set; }
        public int AttributeSetId { get; set; }
        public decimal ProductPrice { get; set; }
        public int AvgRatings { get; set; }
        public int StockQuantity { get; set; }
        public int BrandId { get; set; }
        public string Status { get; set; }
        public int[] AttributteValueIds { get; set; }
    }
}
