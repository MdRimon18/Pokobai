using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlazorInMvc.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSerialController : ControllerBase
    {
        private readonly ProductSerialNumbersService _productSerialNumbersService;
        private readonly SupplierService _supplierService;
        public ProductSerialController(ProductSerialNumbersService productSerialNumbersService, SupplierService supplierService)
        {
            _productSerialNumbersService = productSerialNumbersService;
            _supplierService = supplierService;
        }
        [HttpPost("SaveSerialNumber")]
        public async Task<IActionResult> SaveSerialNumber([FromBody] ProductSerialNumbers productSerialNumber)
        {
            long responseId = 0;
            if (ModelState.IsValid)
            {
                 
                responseId=await _productSerialNumbersService.SaveOrUpdate(productSerialNumber);

                if (responseId == -1)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Serial number already exists!",
                        responseId
                    });
                }
                if(productSerialNumber.TagSupplierId is not null&& productSerialNumber.TagSupplierId>0)
                {
                    Suppliers suppliers = await _supplierService.GetById((long)productSerialNumber.TagSupplierId);
                    if(suppliers is not null)
                    {
                        productSerialNumber.SupplierName = suppliers.SupplrName;
                    }
                }
                productSerialNumber.ProdSerialNmbrId = responseId;
                return Ok(new
                {
                    Data = new
                    {
                        productSerialNumber
                    },
                    code = HttpStatusCode.OK,
                    message = "Serial Number saved successfully!",
                    isSuccess = true,
                    responseId= responseId
                });
                 
            }

            return BadRequest(new { success = false, message = "Validation failed!", errors = ModelState.Values });
        }
        [HttpGet("GetSerialNumber/{id}")]
        public async Task<IActionResult> GetSerialNumber(long id)
        {
            var serialNumber = await _productSerialNumbersService.GetById(id);
            if (serialNumber == null)
            {
                return NotFound(new { success = false, message = "Serial number not found!" });
            }

            return Ok(new
            {
                success = true,
                data = serialNumber
            });
        }
        [HttpGet("GetSerialNumbersByProductId/{productId}")]
        public async Task<IActionResult> GetSerialNumbersByProductId(long productId)
        {
            var serialNumbers = await _productSerialNumbersService.GetByProductId(productId,1,100);
            if (serialNumbers == null)
            {
                return NotFound(new { success = false, message = "Serial number not found!" });
            }

            return Ok(new
            {
                success = true,
                data = serialNumbers
            });
        }
        

    }
}
