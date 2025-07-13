using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorInMvc.Controllers.Api
{
   // [Route("api/[controller]")]
    [ApiController]
    [Route("api/Customer")]
    public class CustomerController : ControllerBase
    {
        //private readonly CustomerService _customerService;
        //public CustomerController(CustomerService customerService )
        //{

        //    _customerService = customerService;
                
        //}
        //[HttpGet]
        //[Route("GetAll")]
        //public async Task<IActionResult> GetAllCustomer(string? search, int page, int pageSize)
        //{
        //    var customers = (await _customerService.Get(null, null, null, null, null, null, null, page, pageSize)).ToList();
        //   if(customers.Count == 0)
        //    {
        //        return Ok(new
        //        {
        //            items = customers,
        //            currentPage = page,
        //            totalPages = 0,
        //            totalRecord = 0
        //        });
        //    }
        //    var totalRecord = customers[0].total_row;
        //    var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

        //    return Ok(new
        //    {
        //        items = customers,
        //        currentPage = page,
        //        totalPages,
        //        totalRecord
        //    });
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customers>> GetById(long id)
        //{
        //    var customer = await _customerService.GetById(id);
        //    if (customer == null)
        //        return NotFound();
        //    return Ok(customer);
        //}

        //[HttpPost]
        //[Route("SaveCustomer")]
        //public async Task<ActionResult<Customers>> SaveCustomer(Customers customer,IFormFile? imageFile)
        //{
        //    if (imageFile != null)
        //    {
        //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        //        var filePath = Path.Combine("wwwroot/images", fileName);
        //        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await imageFile.CopyToAsync(stream);
        //        }
        //        customer.CustImgLink = "/images/" + fileName;
        //    }
        //    // If no new image is provided during update, CustImgLink remains as sent in the form data

        //    // Set audit fields based on create or update
        //    if (customer.CustomerId > 0)
        //    {
        //        // Update
        //        customer.LastModifyDate = DateTime.UtcNow;
        //        customer.LastModifyBy = UserInfo.UserId; // Replace with actual user retrieval logic
        //    }
        //    else
        //    {
        //        // Create
        //        customer.EntryDateTime = DateTime.UtcNow;
        //        customer.EntryBy = UserInfo.UserId; // Replace with actual user retrieval logic
        //    }

        //    var successId = await _customerService.SaveOrUpdate(customer);
        //    if (successId > 0)
        //    {
        //        if (customer.CustomerId == 0)
        //        {
        //            // Create: Return the newly created resource
        //            customer.CustomerId = successId;
        //            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
        //        }
        //        else
        //        {
        //            // Update: Return no content
        //            return NoContent();
        //        }
        //    }
        //    return BadRequest("Failed to save customer");
        //}

        [HttpPost]
        [Route("api/Customer/BulkAction")]
        public async Task<IActionResult> BulkAction([FromBody] BulkActionRequest request)
        {
            //var customers = await _context.Customers.Where(c => customerIds.Contains(c.CustomerId)).ToListAsync();
            //_context.Customers.RemoveRange(customers);
            //await _context.SaveChangesAsync();
            return Ok(new
            {
               request

            });
        }

    }
    public class BulkActionRequest
    {
        public string Action { get; set; }
        public List<long> CustomerIds { get; set; }
    }
}
