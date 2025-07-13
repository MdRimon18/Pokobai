using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Api
{
    //[Route("api/[controller]")]
    [ApiController]

    
    public class InvoiceTypeController : ControllerBase
    {
        private readonly InvoiceTypeService _invoiceTypeService;

        public InvoiceTypeController(InvoiceTypeService invoiceTypeService)
        {
            _invoiceTypeService = invoiceTypeService;
        }

        [HttpGet]
        [Route("api/InvoiceType/GetAll")]
        public async Task<IActionResult> GetInvoiceTypes(string? search, int page, int pageSize)
        {
            var invoiceTypes = await _invoiceTypeService.Get(null, null, null, search, 1, 10);
            var totalRecord = invoiceTypes.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            return Ok(new
            {
                items = invoiceTypes,
                currentPage = page,
                totalPages,
                totalRecord
            });
        }

        [HttpGet]
        [Route("api/InvoiceType/GetById")]
        public async Task<IActionResult> GetInvoiceType(long id)
        {
            var invoiceType = await _invoiceTypeService.GetById(id);
            if (invoiceType == null)
            {
                return NotFound();
            }

            return Ok(invoiceType);
        }

        [HttpPost]
        [Route("api/InvoiceType/ManageInvoiceType")]
        public async Task<IActionResult> ManageInvoiceType([FromBody] InvoiceType request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }

            if (request.Status == "Delete")
            {
                var isDeleted = await _invoiceTypeService.Delete(request.InvoiceTypeId);
                if (isDeleted)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(new { success = false, message = "Failed to delete invoice type" });
            }

            if (request.InvoiceTypeId > 0)
            {
                var invoiceType = await _invoiceTypeService.GetById(request.InvoiceTypeId);
                if (invoiceType == null)
                {
                    return NotFound();
                }

                invoiceType.InvoiceTypeName = request.InvoiceTypeName;
                
                invoiceType.LanguageId = 1; // Replace with actual language ID

                var isUpdated = await _invoiceTypeService.Update(invoiceType);
                if (isUpdated)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(new { success = false, message = "Failed to update invoice type" });
            }
            else
            {
                var invoiceType = new InvoiceType
                {
                    InvoiceTypeName = request.InvoiceTypeName,
                    EntryDateTime = DateTime.Now,
                  
                };

                var invoiceTypeId = await _invoiceTypeService.Save(invoiceType);
                if (invoiceTypeId > 0)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(new { success = false, message = "Failed to create invoice type" });
            }
        }
    }
}
