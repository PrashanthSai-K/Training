using CustomerSupport.Controllers;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;

[TestFixture]
public class ChatControllerTests
{
    private Mock<IChatService> _chatServiceMock;
    private ChatController _controller;

    [SetUp]
    public void Setup()
    {
        _chatServiceMock = new Mock<IChatService>();
        _controller = new ChatController(_chatServiceMock.Object);

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
    public async Task CreateChat_ReturnsOk()
    {
        _chatServiceMock.Setup(x => x.CreateChat("user123", It.IsAny<ChatCreationDto>()))
                        .ReturnsAsync(new Chat());

        var result = await _controller.CreateChat(new ChatCreationDto());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task DeleteChat_ReturnsOk()
    {
        _chatServiceMock.Setup(x => x.DeleteChat("user123", 1)).ReturnsAsync(new Chat());

        var result = await _controller.DeleteChat(1);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetChatById_ReturnsOk()
    {
        _chatServiceMock.Setup(x => x.GetChatById(1)).ReturnsAsync(new Chat());

        var result = await _controller.GetChatById(1);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetChats_ReturnsOk()
    {
        _chatServiceMock.Setup(x => x.GetChats(It.IsAny<ChatQueryParams>()))
                        .ReturnsAsync(new List<Chat>());

        var result = await _controller.GetChats(new ChatQueryParams());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
}
