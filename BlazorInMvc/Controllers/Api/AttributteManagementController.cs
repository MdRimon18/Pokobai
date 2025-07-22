using Domain.Services;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributteManagementController : ControllerBase
    {
        private readonly ProductAttributeService _roductAttributeService;

        public AttributteManagementController(ProductAttributeService productAttributeService)
        {
            _roductAttributeService = productAttributeService;
        }

        [HttpGet("GetAttributes")]
        public IActionResult GetAttributes()
        {
            var attributes = _roductAttributeService.GetAttributes();
            return Ok(attributes);
        }

        [HttpGet("GetAttributeValuesByAttributteId")]
        public IActionResult GetAttributeValuesByAttributteId(int attributteId)
        {
            var values = _roductAttributeService.GetAttributeValuesByAttributteId(attributteId);
            return Ok(values);
        }

        [HttpPost("SaveAttributteWithDetails")]
        public IActionResult SaveAttributteWithDetails([FromBody] AttributteWithDetailsViewModel model)
        {
            var success = _roductAttributeService.SaveAttributteWithDetails(model);
            return Ok(new { success, attributteId = model.AttributteId });
        }

        [HttpGet("DeleteAttribute")]
        public IActionResult DeleteAttribute(int id)
        {
            var success = _roductAttributeService.DeleteAttribute(id);
            return Ok(success);
        }
    }
}
