using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class ProductColorViewModel
    {
        public List<Colors> Color_list { get; set; }
        public Colors color { get; set; } = new Colors();
    }
}
