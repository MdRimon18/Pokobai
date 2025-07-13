using Domain.CommonServices;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Api
{
  //  [Route("api/[controller]")]
    [ApiController]
    public class ProductSubCategoryController : ControllerBase
    {

        private readonly ProductSubCategoryService _productSubCategoryService;
        private readonly ProductCategoryService _productCategoryService;

        public ProductSubCategoryController(ProductSubCategoryService productSubCategoryService,
            ProductCategoryService productCategoryService
          )
        {
            _productSubCategoryService = productSubCategoryService;
            _productCategoryService = productCategoryService;

        }

        [HttpGet]
        [Route("api/ProductSubCategory/GetAll")]
        public async Task<IActionResult> GetAll(string? search, int page, int pageSize)
        {

            // Sending null for all filters
            var users = await _productSubCategoryService.FetchModelList();
            //   var test =users.Where(w=>w.Address is not null).ToList();
            if (users.Count == 0)
            {
                return Ok(new
                {
                    items = users,
                    currentPage = page,
                    totalPages = 0,
                    totalRecord = 0
                });
            }

            var totalRecord = users[0].total_row;
            var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            return Ok(new
            {
                items = users,
                currentPage = page,
                totalPages,
                totalRecord
            });
        }


        [HttpGet]
        [Route("api/ProductSubCategory/ByCategoryId")]
        public async Task<IActionResult> GetProductImageById(long categoryId)
        {
            var subCategoriesByCategoryId = await _productSubCategoryService.Get(
                null,
                null,
                null,
                categoryId,
                null,
                GlobalPageConfig.PageNumber,
                GlobalPageConfig.PageSize
            ); 
            if (subCategoriesByCategoryId == null)
            {
                return NotFound(new { isSuccess = false, message = "Product sub category not found" });
            }

            return Ok(new
            {
                isSuccess = true,
                list = subCategoriesByCategoryId
            });
        }

    }
}
