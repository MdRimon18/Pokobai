using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class ProductViewModel
    {
        //public bool IsEditMode { get; set; }
        public Products Product { get; set; }
        //public string DefaultImage { get; set; }
       
        public List<Products> ProductList { get; set; }
    }   
}
