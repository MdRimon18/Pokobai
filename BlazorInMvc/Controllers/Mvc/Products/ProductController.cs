using Domain.CommonServices;
using Domain.Entity;
using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Domain.Services;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.Design;
using System.Drawing.Printing;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ProductService _productService;
        private readonly UnitService _unitService;
        private readonly SupplierService _supplierService;
        private readonly CurrencyService _currencyService;
        private readonly ShippingByService _shippingByService;
        private readonly ColorService _colorService;
        private readonly CountryServiceV2 _countryServiceV2;
        private readonly StatusSettingService _statusSettingService;
        private readonly ProductSubCategoryService _productSubCategoryService;
        private readonly BrandService _brandService;
        private readonly ProductCategoryService _productCategoryService;

        private readonly ProductSizeService _productSizeService;
        private readonly WarehouseService _warehouseService;
        private readonly BodyPartService _bodyPartService;
        private readonly ProductMediaService _productMediaService;
        private readonly ProductSpecificationService _productSpecificationService;
        private readonly ProductSerialNumbersService _productSerialNumbersService;
        private readonly ProductVarientService _productVariantService;
        private readonly ProductCategoryTypeService _productCategoryTypeService;
        public ProductController(IMemoryCache cache,
            ProductService ProductService,
              UnitService unitService,
            SupplierService supplierService,
            CurrencyService currencyService,
            ShippingByService shippingByService,
            ColorService colorService,
            CountryServiceV2 countryServiceV2,
            StatusSettingService statusSettingService,
            ProductSubCategoryService productSubCategoryService,
            BrandService brandService,
            ProductCategoryService productCategoryService,
            
            ProductSizeService productSizeService,
            WarehouseService warehouseService,
            BodyPartService bodyPartService,
            ProductMediaService productMediaService,
            ProductSpecificationService productSpecificationService,
            ProductSerialNumbersService productSerialNumbersService,
            ProductVarientService productVariantService,
            ProductCategoryTypeService productCategoryTypeService

 
          )
        {
            _cache = cache;
            _productService = ProductService;
            _unitService = unitService;
            _supplierService = supplierService;
            _currencyService = currencyService;
            _shippingByService = shippingByService;
            _colorService = colorService;
            _countryServiceV2 = countryServiceV2;
            _statusSettingService = statusSettingService;
            _productSubCategoryService = productSubCategoryService;
            _brandService = brandService;
            _productCategoryService = productCategoryService;

            _productSizeService = productSizeService;
            _warehouseService = warehouseService;
            _bodyPartService = bodyPartService;
            _productMediaService = productMediaService;
            _productSpecificationService = productSpecificationService;
            _productSerialNumbersService= productSerialNumbersService;
            _productVariantService = productVariantService;
            _productCategoryTypeService = productCategoryTypeService;
        }

        public async Task<IActionResult> Products()
        {

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Products"); // Return partial view for AJAX requests
            }

            return View("Products");

        }

      
        public async Task<IActionResult> Index(bool isPartial = false, long id = 0)
        {
           

         

            var viewModel = new ProductViewModel();
            viewModel.Product = await LoadDDL(new Domain.Entity.Settings.Products());
           
                viewModel.Product.ProductId = id;
               viewModel.ProductList = await FetchModelList();
            
            if (isPartial)
            {
                return PartialView("Index", viewModel);
            }



            viewModel.Product.ProductCategoryTypeList = _productService.GetProductCategoryTypeList();



            return View("Index", viewModel);

        }


        public async Task<List<Domain.Entity.Settings.Products>> FetchModelList()
        {
            //var list = await _productService.Get(
            //    null,
            //    null,
            //    null,
            //    "",
            //    GlobalPageConfig.PageNumber,
            //    GlobalPageConfig.PageSize
            //);
            var list = (await _productService.Get(User.GetCompanyId(), null, null, null, null, null,
                null, null, null, null, null, null, null,
                null, null, null, null, GlobalPageConfig.PageNumber,
                GlobalPageConfig.PageSize)).ToList();

            return list.ToList(); // Convert and return as List<Unit>
        }
      
        public async Task<Domain.Entity.Settings.Products> LoadDDL(Domain.Entity.Settings.Products obj)
        {
            long companyId = User.GetCompanyId();
            var ddl = _productService.GetProductCreateDropdowns(companyId);
            // Populate dropdown fields from cached data
            obj.SupplierList = ddl.SupplierList;
            //obj.UnitList = cachedData.UnitList;
            // obj.CurrencyList = cachedData.CurrencyList;
            obj.ShippingByList = ddl.ShippingByList;
            obj.ColorList = ddl.ColorList;
            //  obj.CountryList = cachedData.CountryList;
            // obj.StatusSettingList = ddl.StatusSettingList;
            //obj.ImportStatusSettingList = cachedData.ImportStatusSettingList;
            obj.ProductSubCategoryList = ddl.ProductSubCategoryList;
            obj.BrandList = ddl.BrandList;
            // obj.ProductCategoryList = cachedData.ProductCategoryList;
            obj.ProductSizeList = ddl.ProductSizeList;
            //  obj.WarehouseList = ddl.WarehouseList;
            obj.BodyParts = ddl.BodyParts;
            // obj.ProductImages = cachedData.ProductImages.Where(w=>w.ProductId==id).ToList();
           // obj.ProductImages = (await _productMediaService.Get(null, null, obj.ProductId, null)).ToList();
            //obj.Specification_list = (await _productSpecificationService.Get(null, null, obj.ProductId, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
           // obj.ProductSerialNumbers_list = (await _productSerialNumbersService.Get(null, null, obj.ProductId, null, null, null, null, null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
           // obj.ProductVariants = new List<ProductVariantViewModel>();//(await _productVariantService.ProductVarientsByProductId(id));

            obj.ProductSubCategoryList = ddl.ProductSubCategoryList;
            obj.ProductCategoryList = ddl.ProductCategoryList;
            obj.UnitList = ddl.UnitList;
            //obj.AttributeValueList = (_productVariantService.GetAttributeValues(companyId).Select(a => new SelectListItem
            //{
            //    Value = a.AttributeValueId.ToString(),
            //    Text = a.AttrbtValue
            //}).ToList());

            obj.ProductCategoryTypeList = _productService.GetProductCategoryTypeList(companyId);
          
            return obj;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdateProductBasicInfo(Domain.Entity.Settings.Products model)
        {
            model.BranchId = CompanyInfo.BranchId;
            //ViewData["RenderLayout"] = null;
            // var viewModel = new ProductViewModel();
            if (!ModelState.IsValid)
            {
                // ViewData["RenderLayout"] = true; // Flag to include layout
                // Retrieve dropdown data from the cache
                 
                await LoadDDL(model);
                 
                if (model.ProdCtgId > 0)
                {
                    var subCategories = await _productSubCategoryService.Get(
                        null, null, null, model.ProdCtgId, null,
                        GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize
                    );
                    model.ProductSubCategoryList = subCategories.ToList().Select(s=> new ProductSubCategoryDto
                    {
                        ProdSubCtgId = s.ProdSubCtgId,
                        ProdSubCtgName = s.ProdSubCtgName
                    }).ToList(); 
                }
                Response.StatusCode = 400;

                //viewModel.ProductList = await FetchModelList();
                //viewModel.Product = model;

                //return PartialView("Index", viewModel);


                // Return the AddForm partial view with validation errors
                return PartialView("_AddForm", model); // Returning partial view directly
            }

            try
            {
                model.CompanyId = User.GetCompanyId();
                long responseId = await _productService.SaveOrUpdate(model);
                if (responseId > 0) {
                    
                   await _productCategoryTypeService.AddOrUpdateAsync(model.CompanyId, responseId,model.ProductCategoryTypeIds);
                }
                if (responseId == -1)
                {
                    //model.rowsAffected = -1;
                    model.ProductId = 0;
                    Response.StatusCode = 409;
                    return PartialView("_AddForm", model); // Returning partial view directly

                }

                var list = await FetchModelList();
                return PartialView("_SearchResult", list); // Returning partial view directly
            }
            catch (Exception ex)
            {

              
                 model = await LoadDDL(model);
               
                if (model.ProdCtgId > 0)
                {
                    var subCategories = await _productSubCategoryService.Get(
                        null, null, null, model.ProdCtgId, null,
                        GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize
                    );
                    model.ProductSubCategoryList = subCategories.ToList().Select(s => new ProductSubCategoryDto
                    {
                        ProdSubCtgId = s.ProdSubCtgId,
                        ProdSubCtgName = s.ProdSubCtgName
                    }).ToList();
                }

                Response.StatusCode = 500;

                //viewModel.ProductList = await FetchModelList();
                //viewModel.Product = model;


                //return PartialView("Index", viewModel);

                // In case of an error, render the AddForm partial view again
                return PartialView("_AddForm", model); // Returning partial view directly
            }
        }


        [HttpGet]
        public async Task<IActionResult> LoadEditModeData(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
               long companyId=User.GetCompanyId();
                Domain.Entity.Settings.Products obj = (await _productService.GetById(companyId, id));
                if (obj == null)
                {
                    return NotFound(); // Handle case where product doesn't exist
                }

                await   LoadDDL(obj);
                obj.ProductImages=(await _productMediaService.Get(null, null, id, null)).ToList();
                obj.Specification_list =(await _productSpecificationService.Get(null, null, id, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
                obj.ProductSerialNumbers_list = (await _productSerialNumbersService.Get(null, null, id, null, null, null, null, null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
                obj.ProductVariants = new List<ProductVariantViewModel>();//(await _productVariantService.ProductVarientsByProductId(id));
             
                obj.AttributeValueList = (_productVariantService.GetAttributeValues(companyId).Select(a => new SelectListItem
                {
                    Value = a.AttributeValueId.ToString(),
                    Text = a.AttrbtValue
                }).ToList());

              obj.ProductCategoryTypeList = _productService.GetProductCategoryTypeList(companyId);

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> LoadSimilarEditModeData(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Domain.Entity.Settings.Products obj = (await _productService.GetById(User.GetCompanyId(), id));

            if (obj == null)
            {
                return NotFound(); // Handle case where product doesn't exist
            }
            obj.ProductId = 0;
            await LoadDDL(obj);
            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewForm()
        {
            // Create a new instance of ProductSze
            Domain.Entity.Settings.Products obj = new();
            await LoadDDL(obj);
          
            // Return the partial view with the populated model
            return PartialView("_AddForm", obj);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid ID provided.");
            }

            try
            {
                if (id > 0)
                {
                    var isDeleted = await _productService.Delete(User.GetCompanyId(), id);

                }

            }
            catch (Exception)
            {
                // It's better to log the exception and provide a meaningful response to the user
                return StatusCode(500, "An error occurred while deleting the pipeline.");
            }



            var list = await FetchModelList();

            return PartialView("_SearchResult", list);


        }

        [HttpPost]
        
        public async Task<IActionResult> SaveProductImage(Domain.Entity.Settings.Products model)
        {
            if (!ModelState.IsValid)
            {
                // ViewData["RenderLayout"] = true; // Flag to include layout
                // Retrieve dropdown data from the cache
               
                 await LoadDDL(model);
                
                Response.StatusCode = 400;

                //viewModel.ProductList = await FetchModelList();
                //viewModel.Product = model;

                //return PartialView("Index", viewModel);


                // Return the AddForm partial view with validation errors
                return PartialView("_AddForm", model); // Returning partial view directly
            }

            try
            {

                model.CompanyId = User.GetCompanyId();
                long responseId = await _productService.SaveOrUpdate(model);
                if (responseId == -1)
                {
                    //model.rowsAffected = -1;
                    model.ProductId = 0;
                    Response.StatusCode = 409;
                    return PartialView("_AddForm", model); // Returning partial view directly

                }

                var list = await FetchModelList();
                return PartialView("_SearchResult", list); // Returning partial view directly
            }
            catch (Exception ex)
            {

                // Retrieve dropdown data from the cache
                var cachedData = _cache.Get<Domain.Entity.Settings.Products>("ProductDropdownData");
                if (cachedData != null)
                {
                    model = cachedData;
                }
                else
                {
                    model = await LoadDDL(model);
                }


                Response.StatusCode = 500;
 
                return PartialView("_AddForm", model); // Returning partial view directly
            }
        }


    }
}
