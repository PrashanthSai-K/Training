using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using CustomerSupport.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace CustomerSupport.Tests.Services
{
    [TestFixture]
    public class TokenServiceTests
    {
        private TokenService _tokenService = null!;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Keys:JwtTokenKey", "supersecurekey_must_be_32bytes!!" }
                }).Build();

            _tokenService = new TokenService(config);
        }

        [Test]
        public void GenerateAccessToken_ShouldReturnToken()
        {
            var user = new User { Username = "user", Roles = "Customer" };
            var token = _tokenService.GenerateAccessToken(user);

            Assert.That(token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void CheckRefreshTokenValidity_ShouldReturnTrue_IfNotExpired()
        {
            var user = new User { Username = "user" };
            var refreshToken = _tokenService.GenerateRefereshToken(user);

            var isValid = _tokenService.CheckRefreshTokenValidity(refreshToken);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void CheckRefreshTokenValidity_ShouldThrowException_ForInvalidToken()
        {
            var invalidToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.invalid.payload";

            Assert.Throws<ArgumentException>(() => _tokenService.CheckRefreshTokenValidity(invalidToken));
        }
    }
}
