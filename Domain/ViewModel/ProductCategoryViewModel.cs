using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class ProductCategoryViewModel
    {
        public List<ProductCategories> ProductCategories { get; set; }
        public ProductCategories ProductCategory { get; set; } = new ProductCategories();
    }
}
