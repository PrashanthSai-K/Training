using CustomerSupport.Controllers;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerSupport.Test.Controllers
{
    [TestFixture]
    public class ChatMessagesControllerTests
    {
        private Mock<IChatMessageService> _chatMessageServiceMock;
        private ChatMessages _controller;

        [SetUp]
        public void Setup()
        {
            _chatMessageServiceMock = new Mock<IChatMessageService>();
            _controller = new ChatMessages(_chatMessageServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "user123")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task CreateMessage_ReturnsOk()
        {
            _chatMessageServiceMock
                .Setup(x => x.CreateTextMessage("user123", 1, It.IsAny<ChatMessageCreateDto>()))
                .ReturnsAsync(new ChatMessage());

            var result = await _controller.CreateMessage(1, new ChatMessageCreateDto());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateMessage_ReturnsOk()
        {
            _chatMessageServiceMock
                .Setup(x => x.EditMessage("user123", 1, 1, It.IsAny<ChatMessageEditDto>()))
                .ReturnsAsync(new ChatMessage());

            var result = await _controller.UpdateMessage(1, 1, new ChatMessageEditDto());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteMessage_ReturnsOk()
        {
            _chatMessageServiceMock
                .Setup(x => x.DeleteMessage("user123", 1, 1))
                .ReturnsAsync(new ChatMessage());

            var result = await _controller.DeleteMessage(1, 1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetMessages_ReturnsOk()
        {
            var expectedMessages = new List<ChatMessage>
            {
                new ChatMessage { Id = 1, Message = "Hello", UserId = "user123" }
            };

            _chatMessageServiceMock
                .Setup(x => x.GetMessages(1, It.IsAny<ChatMessageQueryParams>()))
                .ReturnsAsync(expectedMessages);

            var result = await _controller.GetMessages(1, new ChatMessageQueryParams());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
