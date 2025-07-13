using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlazorInMvc.Controllers.Api
{
   
    [ApiController]
    
    public class BaseController : ControllerBase
     {
        [NonAction]
        public IActionResult ErrorMessage(string message)
        {
            return Ok(new
            {
                result = new { Result = new { } },
                code = (int)HttpStatusCode.NotAcceptable,
                message = message,
                isSuccess = false
            });
        }

        [NonAction]
        public IActionResult SuccessMessage(dynamic data)
        {
            return Ok(new
            {
                result = data,
                code = (int)HttpStatusCode.OK,
                message = "success",
                isSuccess = true
            });
        }

        [NonAction]
        public IActionResult SuccessMessagev2(dynamic data)
        {
            return Ok(new
            {
                result = data,
                code = (int)HttpStatusCode.OK,
                message = "success",
                isSuccess = true
            });
        }

        [NonAction]
        public IActionResult InternalServerError(Exception ex)
        {
            var errorDetails = new
            {
                Message = ex.Message,
                InnerException = ex.InnerException?.Message
            };
            return Ok(new
            {
                result = new { Result = new { } },
                code = (int)HttpStatusCode.InternalServerError,
                message = "An error occurred, please try again later!",
                details = errorDetails,
                isSuccess = false
            });
        }
    }
}
