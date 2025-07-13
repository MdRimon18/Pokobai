using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class UnitsViewModel
    {
        public Unit Unit { get; set; }=new Unit();
        public List<Unit> UnitList { get; set; } = new List<Unit>();
    }
}
