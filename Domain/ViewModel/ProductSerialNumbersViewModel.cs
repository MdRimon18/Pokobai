using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    // Models/ProductSerialNumbersViewModel.cs
    public class ProductSerialNumbersViewModel
    {
        public List<ProductSerialNumbers> ProductSerialNumberList { get; set; } = new List<ProductSerialNumbers>();
        public ProductSerialNumbers ProductSerialObj { get; set; } = new ProductSerialNumbers();
        public bool ShowSuccessAlert { get; set; }
        public bool ShowDangerAlert { get; set; }
        public string SuccessMessage { get; set; }
        public string DangerMessage { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsLoading { get; set; } = true;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalRecord { get; set; }
        public string? ProdSerialNmbr { get; set; }
    }

}
