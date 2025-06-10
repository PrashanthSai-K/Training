using CustomerSupport.Controllers;
using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CustomerSupport.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Test]
        public async Task LoginUser_ShouldReturnLoginResponse()
        {
            // Arrange
            var requestDto = new LoginRequestDto
            {
                Username = "user@example.com",
                Password = "Secure@123"
            };

            var expectedResponse = new LoginResponseDto
            {
                Username = "user@example.com",
                AccessToken = "access-token-123",
                RefreshToken = "refresh-token-123"
            };

            _mockAuthService.Setup(s => s.AuthenticateUser(requestDto))
                            .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.LoginUser(requestDto) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task RefreshSession_ShouldReturnNewLoginResponse()
        {
            // Arrange
            var requestDto = new RefreshSessionRequestDto
            {
                Username = "user@example.com",
                RefreshToken = "refresh-token-123"
            };

            var expectedResponse = new LoginResponseDto
            {
                Username = "user@example.com",
                AccessToken = "new-access-token-456",
                RefreshToken = "new-refresh-token-456"
            };

            _mockAuthService.Setup(s => s.RefreshUserSession(requestDto))
                            .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.RefreshSession(requestDto) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedResponse));
        }
    }
}
