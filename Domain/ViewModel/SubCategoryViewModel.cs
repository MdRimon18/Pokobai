using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class SubCategoryViewModel
    {
        public List<ProductSubCategory> ProductSubCategories { get; set; }
        public ProductSubCategory ProductSubCategory { get; set; } = new ProductSubCategory();
      
    }
}
