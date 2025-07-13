using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class InvoiceViewModel
    {
        public Invoice Invoice { get; set; }=new Invoice();
        public List<InvoiceItems> ItemsList { get; set; } = new List<InvoiceItems>();
        public List<InvoiceItemViewModel> ItemsListViewModel { get; set; } = new List<InvoiceItemViewModel>();
        public List<Products> Products { get; set; } = new List<Products>();
        public List<Products> FilteredItemsOffCanva=new List<Products>();
        public List<NotificationBy> NotificationByList { get; set; } = new List<NotificationBy>();
        public List<InvoiceType> InvoiceTypeList { get; set; } = new List<InvoiceType>();
        public List<ProductCategories> ProductCategoryList { get; set; } = new List<ProductCategories>();
        public List<ProductSubCategory> ProductSubCategoryList { get; set; } = new List<ProductSubCategory>();
        public List<PaymentTypes> PaymentTypesList { get; set; } = new List<PaymentTypes>();
        public List<Customers> CustomersList { get; set; } =new List<Customers>();
        public List<ProductSerialNumbers> SerialNumbers { get; set; } = new List<ProductSerialNumbers>();
       
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public long CustomerId { get; set; }
        public string Mobile { get; set; }
        public string SearchQuery { get; set; }
        public string SrchQuery { get; set; }

        public Suppliers Suppliers { get; set; }
    }

}
