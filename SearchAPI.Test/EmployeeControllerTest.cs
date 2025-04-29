using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SearchApp.Api.Controllers;
using SearchApp.Application;
using SearchApp.Domain;
using SearchApp.Domain.Utility;
using SearchApp.Domain.Utility.NotFoundException;
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
        public async Task SearchEmployee_ReturnsBadRequest_WhenInvalidFilter()
        {
            SearchEmployeeRequest query = new();

            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _controller.SearchEmployee(query));
            Assert.Equal("Failed: Invalid input!", exception.Message);
        }

        [Fact]
        public async Task SearchEmployee_ReturnsOkResult_WhenEmployeeFound()
        {
            // Arrange
            SearchEmployeeRequest query = new SearchEmployeeRequest { Name = "Abhishek", Department = "HR", Salary = Convert.ToDecimal("40000") };
            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId); // Generating token with UserId claim
            var expectedemployee = new List<EmployeeSearchResponse>
            {
                new EmployeeSearchResponse { Id = "E450B587-5555-47AB-A11A-EEEFC9B6FEDC", Name = "Abhishek", Department = "HR" }
            };

            // Mocking Mediator
            _mockMediator
                .Setup(m => m.Send(It.IsAny<EmployeeSearchCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedemployee);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.SearchEmployee(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result).Value;
            var returnedemployee = Assert.IsType<List<EmployeeSearchResponse>>(okResult);
            Assert.Equal(expectedemployee.Count, returnedemployee.Count);
        }

        [Fact]
        public async Task SearchEmployee_ReturnsNotFound_WhenEmployeeNotFound()
        {
            string UserId = Guid.NewGuid().ToString(); // Random UserId
            SearchEmployeeRequest query = new SearchEmployeeRequest { Name = "Test Employee Search" };

            string token = GenerateAccessToken(UserId);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<EmployeeSearchCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EmployeeSearchResponse>()); // No data found

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.SearchEmployee(query));
            Assert.Equal("Failed: Data not found!", exception.Message);
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