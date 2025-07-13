using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.ResponseModel;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Api
{
     
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductCategoryController(ProductCategoryService productCategoryService
          )
        {
            _productCategoryService = productCategoryService;

        }

        [HttpGet]
        [Route("api/ProductCategory/GetAll")]
        public async Task<IActionResult> GetAll(long companyId,string? search, int page, int pageSize)
        {
            CompanyInfo.CompanyId = companyId;
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            // Sending null for all filters
            var categories = await _productCategoryService.FetchModelList();
            var response = categories.Select(c => new ProductCategoriesResponse
            {
                ProdCtgId = c.ProdCtgId,
                ProdCtgName = c.ProdCtgName,
                ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) && !c.ImageUrl.StartsWith(baseUrl)
                        ? baseUrl + c.ImageUrl
                        : c.ImageUrl,
            }).ToList();
            //   var test =users.Where(w=>w.Address is not null).ToList();
            if (response.Count == 0)
            {
                return Ok(new
                {
                    items = response,
                    currentPage = page,
                    totalPages = 0,
                    totalRecord = 0
                });
            }

            var totalRecord = response[0].total_row;
            var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            return Ok(new
            {
                items = response,
                currentPage = page,
                totalPages,
                totalRecord
            });
        }

        

    }
}
