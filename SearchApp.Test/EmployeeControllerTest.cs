using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SearchApp.Api.Controllers;
using SearchApp.Application;
using SearchApp.Application.Queries;
using SearchApp.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace SearchApp.Test
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
        public async Task SearchEmployee_ReturnsOkResult_WhenEmployeeFound()
        {
            // Arrange
            SearchEmployeeRequest query = new SearchEmployeeRequest { Name = "Abhishek", Department = "HR", Salary = Convert.ToDecimal("75000") };
            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId); // Generating token with UserId claim
            var expectedBooks = new List<EmployeeSearchResponse>
            {
                new EmployeeSearchResponse { Id = "296FCADB-3DCF-47DE-8EF0-C9A8FCA0F74C", Name = "Abhishek", Department = "HR" }
            };

            // Mocking Mediator
            _mockMediator
                .Setup(m => m.Send(It.IsAny<EmployeeSearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedBooks);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.SearchEmployee(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<EmployeeSearchResponse>>(okResult.Value);
            Assert.Equal(expectedBooks.Count, returnedBooks.Count);
        }

        [Fact]
        public async Task SearchEmployee_ReturnsBadRequest_WhenInvalidFilter()
        {
            SearchEmployeeRequest query = new ();

            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _controller.SearchEmployee(query));
            Assert.Equal("Invalid filter input. Allowed values are: title, author, summary, tags.", exception.Message);
        }

        [Fact]
        public async Task SearchEmployee_ReturnsNotFound_WhenNoBooksFound()
        {
            string UserId = Guid.NewGuid().ToString(); // Random UserId
            SearchEmployeeRequest query = new SearchEmployeeRequest { Name = "Test Employee Search"};
            
            string token = GenerateAccessToken(UserId);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<EmployeeSearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EmployeeSearchResponse>()); // No data found

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _controller.SearchEmployee(query));
            Assert.Equal("Query cannot be found. Please try again with different query", exception.Message);
        }

        [Fact]
        public async Task GetSearchHistory_ReturnsOkResult_WhenHistoryExists()
        {
            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId);
            var expectedHistory = new List<SearchHistory> { new SearchHistory { /* properties */ } };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SearchHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedHistory);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.GetEmployeeSearchHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedHistory = Assert.IsType<List<SearchHistory>>(okResult.Value);
            Assert.Equal(expectedHistory.Count, returnedHistory.Count);
        }

        [Fact]
        public async Task GetSearchHistory_ReturnsNotFound_WhenNoHistoryFound()
        {
            string UserId = Guid.NewGuid().ToString(); // Random UserId
            string token = GenerateAccessToken(UserId);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SearchHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SearchHistory>()); // No search history

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _controller.GetEmployeeSearchHistory());
            Assert.Equal("There is no history for the user.", exception.Message);
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
