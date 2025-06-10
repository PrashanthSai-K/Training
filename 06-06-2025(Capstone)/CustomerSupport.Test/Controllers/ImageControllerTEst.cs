using CustomerSupport.Controllers;
using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerSupport.Test.Controllers
{
    [TestFixture]
    public class ImageControllerTests
    {
        private Mock<IImageService> _imageServiceMock;
        private ImageController _controller;

        [SetUp]
        public void Setup()
        {
            _imageServiceMock = new Mock<IImageService>();
            _controller = new ImageController(_imageServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "user123"),
                new Claim(ClaimTypes.Role, "Agent")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task UploadImage_ReturnsOk()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "fake image content";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.ContentType).Returns("image/png");

            var imageDto = new ImageUploadDto
            {
                UserId = "user123",
                formFile = fileMock.Object
            };

            _imageServiceMock.Setup(s => s.UploadImage("user123", 1, imageDto))
                             .ReturnsAsync("Image Uploaded");

            // Act
            var result = await _controller.UploadImage(1, imageDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo("Image Uploaded"));
        }

        [Test]
        public async Task DownloadImage_ReturnsOk()
        {
            var response = new ImageReponseDto()
            {
                ImageName = "sample.png",
                ImageUrl = "base64url"
            };
            // Arrange
            _imageServiceMock.Setup(s => s.DownloadImage("user123", 1, "test.png"))
                             .ReturnsAsync(response);

            // Act
            var result = await _controller.DownloadImage(1, "test.png");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(response));
        }
    }
}
