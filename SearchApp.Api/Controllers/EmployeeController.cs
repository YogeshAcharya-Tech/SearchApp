using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchApp.Application;
using SearchApp.Application.Queries;
using SearchApp.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SearchApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ISender sender) : ControllerBase
    {
        /// <summary> Add new employees
        /// </summary>
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeEntity employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ModelStateExtension.AllErrors(ModelState))));
            }

            var result = await sender.Send(new AddEmployeeCommand(employee));
            return Ok(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), result, 200));
        }

        /// <summary> Get all active employees available in emp management database
        /// </summary>
        [HttpPost("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await sender.Send(new GetAllEmployeesQuery());
            
            if (result == null || !result.Any())
            {
                return NotFound(new ApiResponse(404, new ApiError("Failed: Data not found")));
            }

            return Ok(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), result, 200));
        }
        
        /// <summary> Get specific employee by userid
        /// </summary>
        [HttpGet("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest(new ApiResponse(400, new ApiError("Failed: UserId is required", ModelStateExtension.AllErrors(ModelState))));
            }
            var result = await sender.Send(new GetEmployeeByIdQuery(UserId));

            if (result == null)
            {
                return NotFound(new ApiResponse(404, new ApiError("Failed: Data not found")));
            }

            return Ok(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), result, 200));
        }

        /// <summary> Search Employee based on various filter like: Name, CreatedDate, Department, Salary
        /// </summary>
        [HttpPost("SearchEmployee")]
        public async Task<IActionResult> SearchEmployee(SearchEmployeeRequest filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ModelStateExtension.AllErrors(ModelState))));
            }
            
            filter.UserId = GetUserId();

            var result = await sender.Send(new EmployeeSearchQuery(filter));

            if(result == null || !result.Any())
            {
                return NotFound(new ApiResponse(404, new ApiError("Failed: Data not found")));
            }

            return Ok(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), result, 200));
        }

        /// <summary> Get search history saved in DB so that we can easily revisit previous searches  
        /// </summary>

        [HttpPost("GetEmployeeSearchHistory")]
        public async Task<IActionResult> GetEmployeeSearchHistory()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ModelStateExtension.AllErrors(ModelState))));
            }
            
            string UserId = GetUserId();

            var result = await sender.Send(new SearchHistoryQuery(UserId));

            if (result == null || !result.Any())
            {
                return NotFound(new ApiResponse(404, new ApiError("Failed: Data not found")));
            }

            return Ok(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), result, 200));
        }

        /// <summary>
        /// Get UserId from the provided JWT token.
        /// </summary>
        /// <param name="token">From JWT token claim get UserId.</param>
        /// <returns>UserId.</returns>
        private string GetUserId()
        {
            // Get UserId from token claim
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Get the user id from claim
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing UserId in the token.");
            }

            return userId;
        }
    }
}
