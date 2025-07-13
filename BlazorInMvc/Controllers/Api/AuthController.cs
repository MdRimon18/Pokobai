using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlazorInMvc.Controllers.Api
{
   // [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        public AuthController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("api/Auth/Register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input. Please check your data." });
            }

            long userId = await _userService.SaveOrUpdate(model);

            return Ok(new
            {
                userId = userId,
                code = (int)HttpStatusCode.OK,
                message = "success",
                isSuccess = true
            });
        }
    }
}
