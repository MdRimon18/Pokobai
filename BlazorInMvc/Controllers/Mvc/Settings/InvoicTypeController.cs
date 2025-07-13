using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorInMvc.Controllers.Mvc.Settings
{
    public class InvoicTypeController : Controller
    {
        private readonly InvoiceTypeService _invoiceTypeService;
        public InvoicTypeController(InvoiceTypeService invoiceTypeService)
        {
            _invoiceTypeService= invoiceTypeService;
        }
      
        public IActionResult Index()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Index",new List<InvoiceType>()); // Return partial view for AJAX requests
            }

            return View("Index",new List<InvoiceType>());
            
        }
        public async Task<IActionResult> LoadTable(int page = 1,
            int pageSize = 10, string search = "",  
            string sortColumn = "InvoiceTypeId",
            string sortDirection = "desc")
        {
            var invoiceTypes = await _invoiceTypeService.GetInvoiceTypesAsync(page, pageSize, search, sortColumn, sortDirection);
            int totalRecords = invoiceTypes.Any() ? invoiceTypes.First().TotalCount : 0;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var viewModel = new InvoiceTypeViewModel
            {
                InvoiceTypes = invoiceTypes,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Search = search,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            return PartialView("_InvoiceList", viewModel);
        }
        [HttpGet]
        public async Task<bool> RemoveData(Guid id)
        {

            try
            {
                if (id == Guid.Empty)
                {
                    return false;
                }
                else
                {
                    var model = await _invoiceTypeService.DeleteByKey(id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpPost]
        public async Task<IActionResult> ManageInvoiceType([FromBody] InvoiceType request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }

            if (request.InvoiceTypeId > 0)
            {
                var invoiceType = await _invoiceTypeService.GetById(request.InvoiceTypeId);
                if (invoiceType == null)
                {
                    return NotFound(new { success = false, message = MessageManager.NotFound });
                }

                invoiceType.InvoiceTypeName = request.InvoiceTypeName;
                invoiceType.LanguageId = 1; // Replace with actual language ID

                var isUpdated = await _invoiceTypeService.Update(invoiceType);
                if (isUpdated)
                {
                    return Json(new { success = true , message = $"{request.InvoiceTypeName} {MessageManager.UpdateSuccess}" });
                }
                return BadRequest(new { success = false,message = MessageManager.UpdateFaild });
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
                    return Json(new { success = true,message= $"{request.InvoiceTypeName} {MessageManager.SaveSuccess}" });
                }
                if(invoiceTypeId == -1)
                {
                    return Json(new { success = false, message =$"{request.InvoiceTypeName} {MessageManager.Exist}"});
                }
                return BadRequest(new { success = false, message = MessageManager.SaveFaild });
            }
        }
    }
}
