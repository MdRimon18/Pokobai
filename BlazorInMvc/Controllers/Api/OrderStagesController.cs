using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStagesController : ControllerBase
    {
        private readonly OrderStageService _orderStageService;

        public OrderStagesController(OrderStageService orderStageService)
        {
            _orderStageService = orderStageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stages = await _orderStageService.GetAllAsync();
            return Ok(stages);
        }
    }
}
