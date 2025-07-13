using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Inventory
{
    public class ProductVariant:BaseEntity
    {
        public long ProductVariantId { get; set; }
        public long ProductId { get; set; }
        public string? SkuNumber { get; set; } = "";
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public int? StockQuantity { get; set; }
        public string? BodyPartName { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsPrimary { get; set; }
        public int Position { get; set; }
        public IFormFile? file { get; set; }
        //public DateTime? EntryDateTime { get; set; }
        //public long? EntryBy { get; set; }
        //public DateTime? LastModifyDate { get; set; }
        //public long? LastModifyBy { get; set; }
        //public DateTime? DeletedDate { get; set; }
        //public long? DeletedBy { get; set; }
        //public string Status { get; set; }
    }

}
