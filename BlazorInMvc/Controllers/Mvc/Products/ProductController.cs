using Domain.CommonServices;
using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Domain.Services;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Drawing.Printing;

namespace BlazorInMvc.Controllers.Mvc.Products
{
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
        private readonly ProductVariantService _productVariantService;
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
            ProductVariantService productVariantService

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
            var list = (await _productService.Get(null, null, null, null, null,
                null, null, null, null, null, null, null,
                null, null, null, null, GlobalPageConfig.PageNumber,
                GlobalPageConfig.PageSize)).ToList();

            return list.ToList(); // Convert and return as List<Unit>
        }

        public async Task<Domain.Entity.Settings.Products> LoadDDL(Domain.Entity.Settings.Products model)
        {
            // var model = new Domain.Entity.Settings.Products();
            if (!_cache.TryGetValue("ProductDropdownData", out model))
            {
                if (model == null) { model = new Domain.Entity.Settings.Products(); }
                model.SupplierList = (await _supplierService.Get(null, null, null, null, null, null, 1, 1000)).ToList();
                model.UnitList = (await _unitService.Get(null, null, null, null, 1, 1000)).ToList();
                model.CurrencyList = (await _currencyService.Get(null, null, null, null, null, null, null, null, 1, 1000)).ToList();
                model.ShippingByList = (await _shippingByService.Get(null, null, null, null, 1, 1000)).ToList();
                model.ColorList = (await _colorService.Get(null, null, null, null, 1, 1000)).ToList();
                model.CountryList = (await _countryServiceV2.Get(null, null, null, null, null, null, null, null, null, null, 1, 1000)).ToList();
                model.StatusSettingList = (await _statusSettingService.Get(null, null, null, null, "Product", null, 1, 1000)).ToList();
                model.ImportStatusSettingList = (await _statusSettingService.Get(null, null, null, null, null, null, 1, 1000)).ToList();
                model.ProductSubCategoryList = (await _productSubCategoryService.Get(null, null, null, null, null, 1, 1000)).ToList();
                model.BrandList = (await _brandService.Get(null, null, null, 1, 1000)).ToList();
                model.ProductCategoryList = (await _productCategoryService.Get(null, null, null, null, 1, 1000)).ToList();
                model.ProductSizeList = (await _productSizeService.Get(null, null, null, null, 1, 1000)).ToList();
                model.WarehouseList = (await _warehouseService.Get(null, null, null, null, null, null, null, null, null, 1, 1000)).ToList();
                model.BodyParts = await _bodyPartService.GetBodyPartsAsync();
               
                //  model.ProductImage.BodyParts = model.BodyParts;
                // model.ProductImages =
                // model.Specification_list = (await _productSpecificationService.Get(null, null, null, null, null, 1, 1000)).ToList();

                // Store data in the cache with an expiration time
                _cache.Set("ProductDropdownData", model, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), // Cache expires after 1 hour
                    SlidingExpiration = TimeSpan.FromMinutes(30)            // Resets expiration if accessed within 30 mins
                });
            }


            return model;

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
                var cachedData = _cache.Get<Domain.Entity.Settings.Products>("ProductDropdownData");
                if (cachedData != null)
                {
                    model = cachedData;
                }
                else
                {
                    await LoadDDL(model);
                }
                if (model.ProdCtgId > 0)
                {
                    var subCategories = await _productSubCategoryService.Get(
                        null, null, null, model.ProdCtgId, null,
                        GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize
                    );
                    model.ProductSubCategoryList = subCategories?.ToList() ?? new List<Domain.Entity.Settings.ProductSubCategory>();
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
                if (model.ProdCtgId > 0)
                {
                    var subCategories = await _productSubCategoryService.Get(
                        null, null, null, model.ProdCtgId, null,
                        GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize
                    );
                    model.ProductSubCategoryList = subCategories?.ToList() ?? new List<Domain.Entity.Settings.ProductSubCategory>();
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
            Domain.Entity.Settings.Products obj = (await _productService.GetById(id));
            if (obj == null)
            {
                return NotFound(); // Handle case where product doesn't exist
            }
            // Retrieve dropdown data from the cache
            var cachedData = _cache.Get<Domain.Entity.Settings.Products>("ProductDropdownData");
            if (cachedData != null)
            {
                // Populate dropdown fields from cached data
                obj.SupplierList = cachedData.SupplierList;
                obj.UnitList = cachedData.UnitList;
                obj.CurrencyList = cachedData.CurrencyList;
                obj.ShippingByList = cachedData.ShippingByList;
                obj.ColorList = cachedData.ColorList;
                obj.CountryList = cachedData.CountryList;
                obj.StatusSettingList = cachedData.StatusSettingList;
                obj.ImportStatusSettingList = cachedData.ImportStatusSettingList;
                obj.ProductSubCategoryList = cachedData.ProductSubCategoryList;
                obj.BrandList = cachedData.BrandList;
                obj.ProductCategoryList = cachedData.ProductCategoryList;
                obj.ProductSizeList = cachedData.ProductSizeList;
                obj.WarehouseList = cachedData.WarehouseList;
                obj.BodyParts = cachedData.BodyParts;
               // obj.ProductImages = cachedData.ProductImages.Where(w=>w.ProductId==id).ToList();
                obj.ProductImages=(await _productMediaService.Get(null, null, id, null)).ToList();
                obj.Specification_list =(await _productSpecificationService.Get(null, null, id, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
                obj.ProductSerialNumbers_list = (await _productSerialNumbersService.Get(null, null, id, null, null, null, null, null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
                obj.ProductVariants = (await _productVariantService.Get(null, id, null, null, null, null, null, GlobalPageConfig.PageNumber,
                    GlobalPageConfig.PageSize)).ToList();
            }
            else
            {
                await LoadDDL(obj);
            }

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> LoadSimilarEditModeData(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Domain.Entity.Settings.Products obj = (await _productService.GetById(id));

            if (obj == null)
            {
                return NotFound(); // Handle case where product doesn't exist
            }
            obj.ProductId = 0;
          //  obj.Sku = "";
            // Retrieve dropdown data from the cache
            var cachedData = _cache.Get<Domain.Entity.Settings.Products>("ProductDropdownData");
            if (cachedData != null)
            {
                // Populate dropdown fields from cached data
                obj.SupplierList = cachedData.SupplierList;
                obj.UnitList = cachedData.UnitList;
                obj.CurrencyList = cachedData.CurrencyList;
                obj.ShippingByList = cachedData.ShippingByList;
                obj.ColorList = cachedData.ColorList;
                obj.CountryList = cachedData.CountryList;
                obj.StatusSettingList = cachedData.StatusSettingList;
                obj.ImportStatusSettingList = cachedData.ImportStatusSettingList;
                obj.ProductSubCategoryList = cachedData.ProductSubCategoryList;
                obj.BrandList = cachedData.BrandList;
                obj.ProductCategoryList = cachedData.ProductCategoryList;
                obj.ProductSizeList = cachedData.ProductSizeList;
                obj.WarehouseList = cachedData.WarehouseList;
                obj.BodyParts = cachedData.BodyParts;
                // obj.ProductImages = cachedData.ProductImages.Where(w=>w.ProductId==id).ToList();
                //obj.ProductImages = //(await _productMediaService.Get(null, null, id, null)).ToList();
               // obj.Specification_list = (await _productSpecificationService.Get(null, null, id, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
              //  obj.ProductSerialNumbers_list = (await _productSerialNumbersService.Get(null, null, id, null, null, null, null, null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
            }
            else
            {
                await LoadDDL(obj);
            }

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewForm()
        {
            // Create a new instance of ProductSze
            Domain.Entity.Settings.Products obj = new();

            // Retrieve dropdown data from the cache
            var cachedData = _cache.Get<Domain.Entity.Settings.Products>("ProductDropdownData");
            if (cachedData != null)
            {
                // Populate dropdown fields from cached data
                obj.SupplierList = cachedData.SupplierList;
                obj.UnitList = cachedData.UnitList;
                obj.CurrencyList = cachedData.CurrencyList;
                obj.ShippingByList = cachedData.ShippingByList;
                obj.ColorList = cachedData.ColorList;
                obj.CountryList = cachedData.CountryList;
                obj.StatusSettingList = cachedData.StatusSettingList;
                obj.ImportStatusSettingList = cachedData.ImportStatusSettingList;
                obj.ProductSubCategoryList = cachedData.ProductSubCategoryList;
                obj.BrandList = cachedData.BrandList;
                obj.ProductCategoryList = cachedData.ProductCategoryList;
                obj.ProductSizeList = cachedData.ProductSizeList;
                obj.WarehouseList = cachedData.WarehouseList;
                obj.BodyParts = cachedData.BodyParts;
                obj.ProductImage.BodyParts= cachedData.BodyParts;
            }
            else
            {
                // Load dropdown data if cache is not available
                await LoadDDL(obj);
            }

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
                    var isDeleted = await _productService.Delete(id);

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
                var cachedData = _cache.Get<Domain.Entity.Settings.Products>("ProductDropdownData");
                if (cachedData != null)
                {
                    model = cachedData;
                }
                else
                {
                    await LoadDDL(model);
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
