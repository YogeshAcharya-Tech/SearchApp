using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using SearchApp.Api.Controllers;
using SearchApp.Application;
using SearchApp.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SearchAPI.Test
{
    public class EmployeeControllerTest
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly EmployeeController _controller;

        public EmployeeControllerTest()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new EmployeeController(_mockMediator.Object);
        }

        [Fact]
        public async Task EmployeeController_SearchEmployee_ValidResult()
        {
            // Arrange
            EmployeeFilterEnum FilterKey = EmployeeFilterEnum.Department;
            string FilterValue = "Admin";
            EmployeeSortEnum SortBy = EmployeeSortEnum.Name;
            EmployeeSortOrderEnum SortOrder = EmployeeSortOrderEnum.Asc;
            int PageNumber = 1;
            int PageSize = 1;

            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId); // Generating token with UserId claim

            var ExpectedEmpDetails = new List<EmployeeSearchResponse>
            {
                new EmployeeSearchResponse { Id = "639FD0C1-0DED-464D-A879-093D507CD6E2", Email = "Admin@Yahoo.com", Phone = "8767543423", Name = "Admin" }
            };

            ApiResponse ExpectedResult = new ApiResponse(ResponseMessageEnum.Success.GetDescription(), ExpectedEmpDetails, 200);

            string ExpectedJson = JsonConvert.SerializeObject(ExpectedResult);

            // Mocking Mediator
            _mockMediator
                .Setup(m => m.Send(It.IsAny<EmpSearchByGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedEmpDetails);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.SearchEmployee(FilterKey, FilterValue, SortBy, SortOrder, PageNumber, PageSize);

            // Assert
            var ActualResult = Assert.IsType<OkObjectResult>(result).Value;
            string ActualJson = JsonConvert.SerializeObject(ActualResult);

            Assert.Equal(ExpectedJson, ActualJson);
        }

        [Fact]
        public async Task EmployeeController_SearchEmployee_DataNotFound()
        {
            //AAA
            //Arrange
            EmployeeFilterEnum FilterKey = EmployeeFilterEnum.Salary;
            string FilterValue = "20000000";
            EmployeeSortEnum SortBy = EmployeeSortEnum.Salary;
            EmployeeSortOrderEnum SortOrder = EmployeeSortOrderEnum.Asc;
            int PageNumber = 1;
            int PageSize = 1;

            ApiResponse ExpectedResult = new ApiResponse(404, new ApiError("Failed: Data not found"));
            string ExpectedJson = JsonConvert.SerializeObject(ExpectedResult);

            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId); // Generating token with UserId claim

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            //Action
            var result = await _controller.SearchEmployee(FilterKey, FilterValue, SortBy, SortOrder, PageNumber, PageSize);

            var ActualResult = Assert.IsType<NotFoundObjectResult>(result).Value;
            string ActualJson = JsonConvert.SerializeObject(ActualResult);

            //Assert
            Assert.Equivalent(ExpectedJson, ActualJson);
        }

        [Fact]
        public async Task EmployeeController_SearchEmployeePOSTMethod_DataNotFound()
        {
            //AAA
            //Arrange
            SearchEmployeeRequest query = new();

            ApiResponse ExpectedResult = new ApiResponse(404, new ApiError("Failed: Data not found"));
            string ExpectedJson = JsonConvert.SerializeObject(ExpectedResult);

            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            //Action
            var result = await _controller.SearchEmployee(query);

            var ActualResult = Assert.IsType<NotFoundObjectResult>(result).Value;
            string ActualJson = JsonConvert.SerializeObject(ActualResult);

            //Assert
            Assert.Equivalent(ExpectedJson, ActualJson);
        }

        [Fact]
        public async Task EmployeeController_SearchEmployeePOSTMethod_ValidResult()
        {
            // Arrange
            SearchEmployeeRequest SearchRequest = new SearchEmployeeRequest { Name = "Abhishek", Department = "HR", Salary = Convert.ToDecimal("40000") };
            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId); // Generating token with UserId claim

            var ExpectedEmpDetails = new List<EmployeeSearchResponse>
            {
                new EmployeeSearchResponse { Id = "E450B587-5555-47AB-A11A-EEEFC9B6FEDC", Name = "Abhishek", Department = "HR" }
            };

            ApiResponse ExpectedResult = new ApiResponse(ResponseMessageEnum.Success.GetDescription(), ExpectedEmpDetails, 200);

            string ExpectedJson = JsonConvert.SerializeObject(ExpectedResult);

            // Mocking Mediator
            _mockMediator
                .Setup(m => m.Send(It.IsAny<EmployeeSearchCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedEmpDetails);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.SearchEmployee(SearchRequest);

            // Assert
            var ActualResult = Assert.IsType<OkObjectResult>(result).Value;
            string ActualJson = JsonConvert.SerializeObject(ActualResult);

            Assert.Equal(ExpectedJson, ActualJson);
        }
        private string GenerateAccessToken(string UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisisOurSecretKeyThisisOurSecretKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, UserId),
                new Claim("UserId", UserId),
                new Claim(ClaimTypes.Role, "Employee")
            };
            DateTime dateTimeOffset = DateTime.Now.AddMinutes(Convert.ToInt32("6000"));
            var token = new JwtSecurityToken(
              "Test.com",
              "Test.com",
              claims: claims,
              expires: dateTimeOffset,
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}