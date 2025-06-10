using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using CustomerSupport.Services;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ChatServiceTests
{
    private Mock<IRepository<int, Chat>> _chatRepoMock;
    private Mock<IRepository<int, Agent>> _agentRepoMock;
    private Mock<IRepository<int, Customer>> _customerRepoMock;
    private Mock<IAuditLogService> _auditLogMock;
    private Mock<IMapper> _mapperMock;
    private ChatService _service;

    [SetUp]
    public void Setup()
    {
        _chatRepoMock = new Mock<IRepository<int, Chat>>();
        _agentRepoMock = new Mock<IRepository<int, Agent>>();
        _customerRepoMock = new Mock<IRepository<int, Customer>>();
        _auditLogMock = new Mock<IAuditLogService>();
        _mapperMock = new Mock<IMapper>();

        _service = new ChatService(
            _chatRepoMock.Object,
            _agentRepoMock.Object,
            _customerRepoMock.Object,
            _auditLogMock.Object,
            _mapperMock.Object
        );
    }

    [Test]
    public async Task CreateChat_ValidInput_ReturnsChat()
    {
        var dto = new ChatCreationDto { AgentId = 1, CustomerId = 2 };
        var chat = new Chat { Id = 1, AgentId = 1, CustomerId = 2 };

        _agentRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(new Agent { Id = 1 });
        _customerRepoMock.Setup(r => r.GetById(2)).ReturnsAsync(new Customer { Id = 2 });
        _mapperMock.Setup(m => m.Map<ChatCreationDto, Chat>(dto)).Returns(chat);
        _chatRepoMock.Setup(r => r.Create(chat)).ReturnsAsync(chat);

        var result = await _service.CreateChat("agent@example.com", dto);

        Assert.That(result.AgentId, Is.EqualTo(1));
        _auditLogMock.Verify(a => a.CreateAuditLog(It.IsAny<AuditLog>()), Times.Once);
    }

    [Test]
    public void DeleteChat_Unauthorized_ThrowsException()
    {
        var chat = new Chat { Id = 1, AgentId = 1, Status = "Active" };
        var agentList = new List<Agent> { new Agent { Id = 2, Email = "wrong@example.com" } };

        _chatRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(chat);
        _agentRepoMock.Setup(r => r.GetAll()).ReturnsAsync(agentList);

        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteChat("wrong@example.com", 1));
        Assert.That(ex.Message, Is.EqualTo("User not allowed to delete this chat"));
    }
}
