using CustomerSupport.Exceptions;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Services;
using Moq;
using NUnit.Framework;

namespace CustomerSupport.Tests.Services
{
    public class AuthServiceTests
    {
        private Mock<IRepository<string, User>> _userRepoMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IHashingService> _hashingServiceMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IRepository<string, User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _hashingServiceMock = new Mock<IHashingService>();

            _authService = new AuthService(
                _userRepoMock.Object,
                _tokenServiceMock.Object,
                _hashingServiceMock.Object
            );
        }


        [Test]
        public async Task RefreshUserSession_ValidToken_ReturnsNewAccessToken()
        {
            var request = new RefreshSessionRequestDto
            {
                Username = "john@example.com",
                RefreshToken = "valid_token"
            };

            var user = new User
            {
                Username = "john@example.com",
                RefreshToken = "valid_token"
            };

            _userRepoMock.Setup(r => r.GetById("john@example.com")).ReturnsAsync(user);
            _tokenServiceMock.Setup(t => t.CheckRefreshTokenValidity("valid_token")).Returns(true);
            _tokenServiceMock.Setup(t => t.GenerateAccessToken(user)).Returns("new_access_token");

            var result = await _authService.RefreshUserSession(request);

            Assert.That(result.AccessToken, Is.EqualTo("new_access_token"));
            Assert.That(result.RefreshToken, Is.EqualTo("valid_token"));
        }

        [Test]
        public void RefreshUserSession_InvalidToken_ThrowsException()
        {
            var request = new RefreshSessionRequestDto
            {
                Username = "john@example.com",
                RefreshToken = "invalid_token"
            };

            var user = new User
            {
                Username = "john@example.com",
                RefreshToken = "valid_token"
            };

            _userRepoMock.Setup(r => r.GetById("john@example.com")).ReturnsAsync(user);

            Assert.ThrowsAsync<InvalidTokenException>(() => _authService.RefreshUserSession(request));
        }

        [Test]
        public void RefreshUserSession_ExpiredToken_ThrowsException()
        {
            var request = new RefreshSessionRequestDto
            {
                Username = "john@example.com",
                RefreshToken = "expired_token"
            };

            var user = new User
            {
                Username = "john@example.com",
                RefreshToken = "expired_token"
            };

            _userRepoMock.Setup(r => r.GetById("john@example.com")).ReturnsAsync(user);
            _tokenServiceMock.Setup(t => t.CheckRefreshTokenValidity("expired_token")).Returns(false);

            Assert.ThrowsAsync<InvalidTokenException>(() => _authService.RefreshUserSession(request));
        }
    }
}
