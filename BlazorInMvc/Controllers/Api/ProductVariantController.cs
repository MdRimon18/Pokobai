using Domain.CommonServices;
using Domain.Entity;
using Domain.Entity.Inventory;
using Domain.Helper;
using Domain.Services;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlazorInMvc.Controllers.Api
{
   // [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductVarientService _productVariantService;
        private readonly ProductSpecificationService _productSpecificationService;
        private readonly ProductService _productService;
        public ProductVariantController(ProductVarientService productVariantSerivice,
            ProductSpecificationService productSpecificationService,
            ProductService productService, ILogger<ProductController> logger)
        {
            _productVariantService = productVariantSerivice;
            _productSpecificationService = productSpecificationService;
            _productService = productService;
            _logger = logger;
        }
         

        [HttpPost]
        [Route("api/ProductVariant/SaveOrUpdate")]
        public async Task<IActionResult> SaveOrUpdate([FromForm] ProductVariants model)
        {
            ProductVariantViewModel productVariant = new ProductVariantViewModel();
            if (model.file != null || model.file?.Length > 0)
            {
                // Get the base URL
                //var request = HttpContext.Request;
                //var baseUrl = $"{request.Scheme}://{request.Host}";

                string extension = Path.GetExtension(model.file.FileName);
                var bytes = await new MediaHelper().GetBytes(model.file);

                // Upload file and get its relative path
                var relativePath = MediaHelper.UploadAnyFile(bytes, "/Content/Images", extension);

                model.ImageUrl =relativePath;

            }

            try
            {
                var (success, productVariantId) = await _productVariantService.SaveProductVariantWithProductVariantAttribute(model);
                if (success&&productVariantId>0)
                {
                   productVariant= await _productVariantService.ProductVarientsById(productVariantId);
                }
                //productVariants = (List<ProductVariant>)await _productVariantService.Get(null, model.ProductId, null, null,null,null,null,GlobalPageConfig.PageNumber,
                //    GlobalPageConfig.PageSize);
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    code = HttpStatusCode.InternalServerError,
                    message = "An error occurred while saving the product image. Please try again later.",
                    isSuccess = false
                });
            }

            return Ok(new
            {
                productVariant,
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true
            });
        }

        [HttpGet]
        [Route("api/ProductVariant/GetByVariantIdId")]
        public async Task<IActionResult> GetProductImageById(long productVariantId)
        {
            var productVariant = await _productVariantService.GetVariantForEditAsync(productVariantId);
            if (productVariant == null)
            {
                return NotFound(
                    new {
                    isSuccess = false,
                    message = "Product Variant not found" 
                    }
                    );
            }

            return Ok(new
            {
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true,
                data = productVariant
            });
        }

        [HttpGet]
        [Route("api/GetProductVariantByProductId")]
        public async Task<IActionResult> GetProductVariantByProductId(long productId)
        {
            var productVariants = await _productVariantService.ProductVarientsByProductId(productId);
            if (productVariants == null)
            {
                return NotFound(
                    new
                    {
                        isSuccess = false,
                        message = "Product Variant not found"
                    }
                    );
            }

            return Ok(new
            {
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true,
                productVariants
            });
        }

        [HttpDelete]
        [Route("api/ProductVariant/DeleteProductVariant")]
        public async Task<IActionResult> DeleteProductImage(long productVariantId)
        {
            if (productVariantId <= 0)
            {
                return BadRequest(new { isSuccess = false, message = "Invalid Product Variant Id." });
            }

            try
            {
                var result =  _productVariantService.DeleteProductAttriVariants(productVariantId);
                if (result)
                {
                    return Ok(new { isSuccess = true, message = "Product Variant deleted successfully." });
                }
                else
                {
                    return NotFound(new { isSuccess = false, message = "Product Variant not found." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging library or framework)
                return StatusCode(500, new { isSuccess = false, message = "An error occurred while deleting the product image.", details = ex.Message });
            }
        }

    }
}
