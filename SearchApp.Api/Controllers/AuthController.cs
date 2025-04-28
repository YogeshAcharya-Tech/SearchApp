using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchApp.Application;
using SearchApp.Core;

namespace SearchApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ISender sender) : ControllerBase
    {
        /// <summary> Generating a JWT token if Username and Password is Valid
        /// </summary>
        [HttpPost("GetAccessToken")]
        public async Task<IActionResult> GetAccessToken(AuthRequest AuthRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ModelStateExtension.AllErrors(ModelState))));
            }

            var result = await sender.Send(new AuthCommand(AuthRequest));
            return Ok(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), result, 200));
        }
    }
}
