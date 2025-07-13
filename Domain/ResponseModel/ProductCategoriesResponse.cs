using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseModel
{
    public class ProductCategoriesResponse
    {
        public long ProdCtgId { get; set; }
        public string ProdCtgName { get; set; }
        public string? ImageUrl { get; set; }
        public int total_row { get; set; }
    }
}
