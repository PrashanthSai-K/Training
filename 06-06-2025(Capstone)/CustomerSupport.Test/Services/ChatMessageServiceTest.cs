using System.Threading.Tasks;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.MessageHub;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ChatMessageServiceTests
{
    private Mock<IRepository<int, ChatMessage>> _chatMsgRepoMock;
    private Mock<IRepository<int, Chat>> _chatRepoMock;
    private Mock<IRepository<string, User>> _userRepoMock;
    private Mock<IOtherContextFunctions> _otherFuncsMock;
    private Mock<IHubContext<ChatHub>> _hubContextMock;
    private Mock<IMapper> _mapperMock;
    private ChatMessageService _service;

    [SetUp]
    public void Setup()
    {
        _chatMsgRepoMock = new Mock<IRepository<int, ChatMessage>>();
        _chatRepoMock = new Mock<IRepository<int, Chat>>();
        _userRepoMock = new Mock<IRepository<string, User>>();
        _otherFuncsMock = new Mock<IOtherContextFunctions>();
        _hubContextMock = new Mock<IHubContext<ChatHub>>();
        _mapperMock = new Mock<IMapper>();

        // Fix: Properly mock HubContext Clients.Group().SendAsync(...)
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();
        mockClientProxy
            .Setup(proxy => proxy.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
            .Returns(Task.CompletedTask);

        mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(mockClientProxy.Object);
        _hubContextMock.Setup(h => h.Clients).Returns(mockClients.Object);

        _service = new ChatMessageService(
            _chatMsgRepoMock.Object,
            _chatRepoMock.Object,
            _userRepoMock.Object,
            _otherFuncsMock.Object,
            _hubContextMock.Object,
            _mapperMock.Object
        );
    }

    [Test]
    public async Task CreateTextMessage_Valid_ReturnsMessage()
    {
        var dto = new ChatMessageCreateDto { UserId = "user123", Message = "Hello!" };
        var chatMessage = new ChatMessage { Id = 1, UserId = "user123", ChatId = 10, Message = "Hello!" };

        _chatRepoMock.Setup(r => r.GetById(10)).ReturnsAsync(new Chat { Id = 10 });
        _userRepoMock.Setup(r => r.GetById("user123")).ReturnsAsync(new User { Username = "user123" });
        _otherFuncsMock.Setup(f => f.IsUserInChat(10, "user123")).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map<ChatMessageCreateDto, ChatMessage>(dto)).Returns(chatMessage);
        _chatMsgRepoMock.Setup(r => r.Create(It.IsAny<ChatMessage>())).ReturnsAsync(chatMessage);

        var result = await _service.CreateTextMessage("user123", 10, dto);

        Assert.That(result.Message, Is.EqualTo("Hello!"));
    }

    [Test]
    public void DeleteMessage_Unauthorized_ThrowsException()
    {
        var chatMessage = new ChatMessage { Id = 1, ChatId = 10, UserId = "ownerId", Message = "test" };
        _chatRepoMock.Setup(r => r.GetById(10)).ReturnsAsync(new Chat { Id = 10 });
        _chatMsgRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(chatMessage);

        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteMessage("wrongUser", 10, 1));
        Assert.That(ex.Message, Is.EqualTo("User not authorized to delete message of this chat"));
    }
}
