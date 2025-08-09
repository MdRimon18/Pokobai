using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplrName { get; set; }
    }

    public class ColorDto
    {
        public int ColorId { get; set; }
        public string ColorIdName { get; set; }
    }

    public class SizeDto
    {
        public int ProductSizeId { get; set; }
        public string ProductSizeName { get; set; }
    }

    public class ShippingByDto
    {
        public int ShippingById { get; set; }
        public string ShippingByName { get; set; }
    }

    public class BodyPartDto
    {
        public string BodyPartName { get; set; }
    }

    public class ProductCategoryDto
    {
        public long ProdCtgId { get; set; }
        public string ProdCtgName { get; set; }
    }

    public class ProductSubCategoryDto
    {
        public long ProdSubCtgId { get; set; }
        public string ProdSubCtgName { get; set; }
    }

    public class BrandDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }

    public class UnitDto
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
    }

    public class ProductCreateDropdownsViewModel
    {
        public List<SupplierDto> SupplierList { get; set; }
        public List<ColorDto> ColorList { get; set; }
        public List<SizeDto> ProductSizeList { get; set; }
        public List<ShippingByDto> ShippingByList { get; set; }
        public List<BodyPartDto> BodyParts { get; set; }
        public List<ProductCategoryDto> ProductCategoryList { get; set; }
        public List<ProductSubCategoryDto> ProductSubCategoryList { get; set; }
        public List<BrandDto> BrandList { get; set; }
        public List<UnitDto> UnitList { get; set; }
    }
}
