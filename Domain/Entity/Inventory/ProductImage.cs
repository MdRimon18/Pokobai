using Domain.Entity.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Inventory
{
    public class ProductImage
    {
        public long ProductMediaId { get; set; }

        public Guid? ProductMediaKey { get; set; }

        public long ProductId { get; set; }

        public string? ImageUrl { get; set; }

        public int Position { get; set; }
        
        public string? BodyPartName { get; set; }
        [NotMapped]
        public IEnumerable<BodyPart>? BodyParts { get; set; } = new List<BodyPart>();
        public IFormFile? file { get; set; }
    }
}
