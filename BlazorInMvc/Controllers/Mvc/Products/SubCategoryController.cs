using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    public class SubCategoryController : Controller
    {
        private readonly ProductSubCategoryService _productSubCategoryService;
        private readonly ProductCategoryService _productCategoryService;

        public SubCategoryController(ProductSubCategoryService productSubCategoryService,
            ProductCategoryService productCategoryService
          )
        {
            _productSubCategoryService = productSubCategoryService;
            _productCategoryService = productCategoryService;

        }


        public async Task<IActionResult> Index()
        {
            SubCategoryViewModel viewModel = new SubCategoryViewModel();
            viewModel.ProductSubCategory.ProductCategory_list= (await _productCategoryService.Get(null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
            viewModel.ProductSubCategories = await FetchModelList();
            return PartialView("Index", viewModel);

        }
        public async Task<List<ProductSubCategory>> FetchModelList()
        {
            var list = await _productSubCategoryService.Get(
                null,
                null,
                null,
                null,
                null,
                GlobalPageConfig.PageNumber,
                GlobalPageConfig.PageSize
            );

            return list.ToList(); // Convert and return as List<Unit>
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(ProductSubCategory model)
        {
            if (!ModelState.IsValid)
            {
                SubCategoryViewModel viewModel = new SubCategoryViewModel();
                viewModel.ProductSubCategory.ProductCategory_list = (await _productCategoryService.Get(null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
                viewModel.ProductSubCategories = await FetchModelList();
                Response.StatusCode = 400;
                
                return PartialView("_AddForm", viewModel); // Returning partial view directly
            }

            try
            {
                long responseId = await _productSubCategoryService.SaveOrUpdate(model);
                if (responseId == -1)
                {
                    SubCategoryViewModel viewModel = new SubCategoryViewModel();
                    viewModel.ProductSubCategory.ProductCategory_list = (await _productCategoryService.Get(null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
                    viewModel.ProductSubCategories = await FetchModelList();
                    model.ProdSubCtgId = 0;
                    Response.StatusCode = 409;
                    return PartialView("_AddForm", viewModel); // Returning partial view directly

                }

                var list = await FetchModelList();
                // Return the _SearchResult partial view with the updated list
                return PartialView("_SearchResult", list); // Returning partial view directly
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                SubCategoryViewModel viewModel = new SubCategoryViewModel();
                viewModel.ProductSubCategory.ProductCategory_list = (await _productCategoryService.Get(null, null, null, null, GlobalPageConfig.PageNumber, GlobalPageConfig.PageSize)).ToList();
                viewModel.ProductSubCategories = await FetchModelList();
                // In case of an error, render the AddForm partial view again
                return PartialView("_AddForm", viewModel); // Returning partial view directly
            }
        }


        [HttpGet]
        public async Task<IActionResult> LoadEditModeData(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ProductSubCategory obj = (await _productSubCategoryService.GetById(id));

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewForm()
        {
            ProductSubCategory obj = new();
            if (obj == null)
            {
                return NotFound();
            }

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
                    var isDeleted = await _productSubCategoryService.Delete(id);

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
    }
}
