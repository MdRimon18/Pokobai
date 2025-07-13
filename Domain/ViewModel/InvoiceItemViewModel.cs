using Domain.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class InvoiceItemViewModel
    {
        public long InvoiceItemId { get; set; }
        public long InvoiceId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal DiscountPercentg { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal VatPercent { get; set; }
         
        public string ProdCtgName { get; set; }
        public string ProdName { get; set; }
        public string ProdSubCtgName { get; set; }
        public string UnitName { get; set; }
        public string ImageUrl { get; set; }
        public List<InvoiceItemSerials> SelectedSerialNumbers { get; set; } = new();
    }

    //public class SerialNumberViewModel
    //{
    //    public string SerialNumber { get; set; }
    //    public long ProdSerialNmbrId { get; set; }
    //    public string SupplierOrgName { get; set; } // Optional
    //    public double Rate { get; set; }
    //}

}
