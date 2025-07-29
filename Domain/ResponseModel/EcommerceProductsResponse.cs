using Domain.Entity;
using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseModel
{
    public class EcommerceProductsResponse
    {
        public long ProductId { get; set; }
        public Guid? ProductKey { get; set; }
    
        //public long? ProdCtgId { get; set; } = null;
        //public long? ProdSubCtgId { get; set; }
        
        //public long? UnitId { get; set; } = null;
        public decimal FinalPrice { get; set; }
        public decimal? PreviousPrice { get; set; }
    
      //  public long? CurrencyId { get; set; } = null;
      //  public string? TagWord { get; set; }
    
        public string ProdName { get; set; }
        public string? ManufacturarName { get; set; }
        public string? SerialNmbrOrUPC { get; set; }
        public string? Sku { get; set; }
        //public int? OpeningQnty { get; set; } = null;

      //  public int? AlertQnty { get; set; } = 0;
      //  public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal VatPercent { get; set; } = 0;
        public decimal VatAmount { get; set; } = 0;
        public decimal DiscountPercentg { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
       // public string? BarCode { get; set; }
       // public int? SupplirLinkId { get; set; }
       // public string? ImportedForm { get; set; }
       // public int? ImportStatusId { get; set; }
  //      public DateTime? GivenEntryDate { get; set; }
        public int? WarrentYear { get; set; }
        public string? WarrentyPolicy { get; set; }
       // public int? ColorId { get; set; }
       // public int? SizeId { get; set; }

     //   public int? ShippingById { get; set; } = null;
        public int? ShippingDays { get; set; }
        public string? ShippingDetails { get; set; }
    //    public int? OriginCountryId { get; set; }
        public int? Rating { get; set; }
     //   public int? ProdStatusId { get; set; }
        public string? Remarks { get; set; }
        public string? ProdShortDescription { get; set; }
        public string? ProdDescription { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public long BranchId { get; set; }
        public int StockQuantity { get; set; }
        public decimal? ItemWeight { get; set; }
        public long? WarehouseId { get; set; }
        public string? RackNumber { get; set; }
        public string? BatchNumber { get; set; }
        public long? PolicyId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductHieght { get; set; }
    //    public long? BrandId { get; set; }
        public string? ProdCtgName { get; set; }
        public string? BrandName { get; set; }
        public string? ProdSubCtgName { get; set; }
        public string? UnitName { get; set; }
        public string? CurrencySymbol { get; set; }
        public int total_row { get; set; } = 0;
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public string? ImageUrl { get; set; }
        public List<ProductVariantViewModel> ProductVariants { get; set; } = new List<ProductVariantViewModel>();
     
        public List<SpecificationGroupResponse> Specificationlist { get; set; } = new List<SpecificationGroupResponse>();
        public string? StockStatus { get; set; }

    }
    public class SpecificationGroupResponse
    {
        public string HeaderName { get; set; }
        public List<ProductSpecificationResponse> Specifications { get; set; }
    }
    public class ProductSpecificationResponse
    {
        public long ProdSpcfctnId { get; set; }
        public string SpecificationName { get; set; }
        public string SpecificationDtls { get; set; }
    }
}
