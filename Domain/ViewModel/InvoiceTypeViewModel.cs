using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class InvoiceTypeViewModel
    {   
        public IEnumerable<InvoiceType> InvoiceTypes { get; set; }
        public InvoiceType InvoiceType { get; set; } = new InvoiceType();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }

}
