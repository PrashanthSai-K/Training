using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Services;
using Moq;

[TestFixture]
public class AgentServiceTests
{
    private Mock<IRepository<int, Agent>> _agentRepoMock;
    private Mock<IRepository<string, User>> _userRepoMock;
    private Mock<IHashingService> _hashingMock;
    private Mock<IAuditLogService> _auditMock;
    private Mock<IOtherContextFunctions> _otherContextMock;
    private IMapper _mapper;
    private AgentService _service;

    [SetUp]
    public void Setup()
    {
        _agentRepoMock = new Mock<IRepository<int, Agent>>();
        _userRepoMock = new Mock<IRepository<string, User>>();
        _hashingMock = new Mock<IHashingService>();
        _auditMock = new Mock<IAuditLogService>();
        _otherContextMock = new Mock<IOtherContextFunctions>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AgentRegisterDto, Agent>();
            cfg.CreateMap<AgentRegisterDto, User>();
            cfg.CreateMap<AgentUpdateDto, Agent>();
        });
        _mapper = config.CreateMapper();

        _service = new AgentService(_agentRepoMock.Object, _userRepoMock.Object, _mapper, _auditMock.Object,_otherContextMock.Object, _hashingMock.Object);
    }

    [Test]
    public async Task CreateAgent_ShouldHashPasswordAndCreateAgent()
    {
        var dto = new AgentRegisterDto { Email = "agent@test.com", Name = "Test", Password = "secret" };

        _agentRepoMock.Setup(r => r.Create(It.IsAny<Agent>())).ReturnsAsync(new Agent());
        _userRepoMock.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync(new User());
        _hashingMock.Setup(h => h.HashData("secret")).Returns("hashed");
        _otherContextMock.Setup(o => o.IsUsernameExists(It.IsAny<string>())).ReturnsAsync(false);

        var result = await _service.CreateAgent(dto);

        Assert.That(result, Is.Not.Null);
        _userRepoMock.Verify(r => r.Create(It.Is<User>(u => u.Password == "hashed" && u.Roles == "Agent")), Times.Once);
    }

    [Test]
    public void UpdateAgent_InvalidUser_ShouldThrow()
    {
        var agent = new Agent { Id = 1, Email = "other@test.com" };
        _agentRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(agent);

        var dto = new AgentUpdateDto { Name = "Updated" };

        Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.UpdateAgent("wrong@test.com", 1, dto));
    }
}
