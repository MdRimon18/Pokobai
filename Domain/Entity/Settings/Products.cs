

using Domain.CommonServices;
using Domain.Entity.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entity.Settings
        
{
    public class Products:BaseEntity
    {
        [Key]
        public long ProductId { get; set; }
        public Guid? ProductKey { get; set; }
        public long CompanyId { get; set; }

        [Required(ErrorMessage = "Product Category is required")]
        public long? ProdCtgId { get; set; } = null; 
        public long? ProdSubCtgId { get; set; }
        [Required(ErrorMessage = "Unit type is required")]
        public long? UnitId { get; set; } = null;
        public decimal FinalPrice { get; set; }
        public decimal? PreviousPrice { get; set; }
        //[Required(ErrorMessage = "Currency is required")]
        public long? CurrencyId { get; set; } = null; 
        public string? TagWord { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters")]
        public string ProdName { get; set; }
        public string? ProdSlug { get; set; }
        public string? ManufacturarName { get; set; }
        public string? SerialNmbrOrUPC { get; set; }
        [Required(ErrorMessage = "SKU is required")]
        [StringLength(100, ErrorMessage = "SKU cannot exceed 100 characters")]
        public string? Sku { get; set; }
        [Required(ErrorMessage = "Opening quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Opening quantity must be greater than zero")]
        public int? OpeningQnty { get; set; } = null;
         
        public int? AlertQnty { get; set; } = 0;
        [Required(ErrorMessage = "Buying price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Buying price must be greater than zero")]
        public decimal BuyingPrice { get; set; }
        [Required(ErrorMessage = "Selling price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than zero")]
        public decimal SellingPrice { get; set; }
        public decimal VatPercent { get; set; } = 0;
        public decimal VatAmount { get; set; } = 0;
        public decimal DiscountPercentg { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public string? BarCode { get; set; }
        public int? SupplirLinkId { get; set; }
        public string? ImportedForm { get; set; }
        public int? ImportStatusId { get; set; }
        public DateTime? GivenEntryDate { get; set; } 
        public int? WarrentYear { get; set; }
        public string? WarrentyPolicy { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
 
        public int? ShippingById { get; set; }=null;
        public int? ShippingDays { get; set; }
        public string? ShippingDetails { get; set; }
        public int? OriginCountryId { get; set; }
        public int? Rating { get; set; }
        public int? ProdStatusId { get; set; }
        public string? Remarks { get; set; }
        public string? ProdShortDescription { get; set; }
        public string? ProdDescription { get; set; }
        public DateTime? ReleaseDate { get; set; } 
        public long BranchId { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required")]
        public int StockQuantity { get; set; }
        public decimal? ItemWeight { get; set; }
        public long? WarehouseId { get; set; }
        public string? RackNumber { get; set; }
        public string? BatchNumber { get; set; }
        public long? PolicyId { get; set; }
        [Required(ErrorMessage = "Product Code is required")]
        public string? ProductCode { get; set; }
        public string? ProductHieght { get; set; }
        public long? BrandId { get; set; }
        //public DateTime? EntryDateTime { get; set; }
        //public long? EntryBy { get; set; }
        //public DateTime? LastModifyDate { get; set; }
        //public long? LastModifyBy { get; set; }
        //public DateTime? DeletedDate { get; set; }
        //public long? DeletedBy { get; set; }
       // public string Status { get; set; }
        public string? ProdCtgName { get; set; }
        public string? BrandName { get; set; }
        public string? ProdSubCtgName { get; set; }
        public string? UnitName {  get; set; }
        public string? CurrencySymbol { get; set; }
        public string? StockStatus { get; set; }
        public string? ProductCategoryType { get; set; }

        [NotMapped] 
        public int total_row { get; set; } = 0;
        [NotMapped]
        public long? ProductVariantId { get; set; }
        [NotMapped]
        public List<Unit> UnitList { get; set; } = new List<Unit>();
        [NotMapped]
        public List<Suppliers> SupplierList { get; set; } = new List<Suppliers>();
        [NotMapped]
        public List<Currency> CurrencyList { get; set; } = new List<Currency>();
        [NotMapped]
        public List<ShippingBy> ShippingByList { get; set; } = new List<ShippingBy>();
        [NotMapped]
        public List<Colors> ColorList { get; set; } = new List<Colors>();
        [NotMapped]
        public List<CountryV2> CountryList { get; set; } = new List<CountryV2>();
        [NotMapped]
        public List<StatusSetting> StatusSettingList { get; set; } = new List<StatusSetting>();
        [NotMapped]
        public List<StatusSetting> ImportStatusSettingList { get; set; } = new List<StatusSetting>();
        [NotMapped]
        public List<ProductSubCategory> ProductSubCategoryList { get; set; }= new List<ProductSubCategory>();
        [NotMapped]
        public List<Brands> BrandList { get; set; } = new List<Brands>();
        [NotMapped]
        public List<ProductCategories> ProductCategoryList { get; set; } = new List<ProductCategories>();
        [NotMapped]
        public List<ProductSze> ProductSizeList { get; set; } = new List<ProductSze>();
        [NotMapped]
        public List<Warehouse> WarehouseList { get; set; }=new List<Warehouse>();
        [NotMapped]
        public IEnumerable<BodyPart> BodyParts { get; set; }=new List<BodyPart>();

        [NotMapped]
         public  List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        [NotMapped]
        public ProductImage ProductImage { get; set; } = new ProductImage();
        [NotMapped]
        public  List<ProductSpecifications> Specification_list = new List<ProductSpecifications>();
        [NotMapped]
        public ProductSpecifications productSpecification = new ProductSpecifications();
        [NotMapped]
        public ProductSerialNumbers ProductSerialNumber = new ProductSerialNumbers();
        [NotMapped]
        public List<ProductSerialNumbers> ProductSerialNumbers_list = new List<ProductSerialNumbers>();
        [NotMapped]
        public string? ImageUrl { get; set; }
        //[NotMapped]
        //public string? VariantImageUrl { get; set; }

        [NotMapped]
        public List<ProductVariantViewModel> ProductVariants { get; set; } = new List<ProductVariantViewModel>();
        [NotMapped]
        public List<ProductVariantDto> ProductVariantsEcom { get; set; } = new List<ProductVariantDto>();

        [NotMapped]
        public ProductVariants ProductVariant { get; set; } = new ProductVariants();
        public string? ProductShortSpecification { get; set; }
        public List<string>? TagList { get; set; }=new List<string>();

        public List<Products> RelevantProducts { get; set; }=new List<Products>();
  
        [NotMapped]
        public List<SelectListItem> AttributeValueList { get; set; } = new();
        [NotMapped]
        public List<SelectListItem> ProductCategoryTypeList { get; set; } = new();
        public List<string> ProductCategoryTypeIds { get; set; } = new();
    }
}
