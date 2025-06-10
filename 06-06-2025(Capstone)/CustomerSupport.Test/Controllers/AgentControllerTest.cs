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
public class AgentControllerTests
{
    private Mock<IAgentService> _agentServiceMock;
    private AgentController _controller;

    [SetUp]
    public void Setup()
    {
        _agentServiceMock = new Mock<IAgentService>();
        _controller = new AgentController(_agentServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "agentUser")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Test]
    public async Task RegisterAgent_ReturnsCreated()
    {
        var dto = new AgentRegisterDto();
        _agentServiceMock.Setup(x => x.CreateAgent(dto)).ReturnsAsync(new Agent());

        var result = await _controller.RegisterAgent(dto);

        Assert.That(result, Is.InstanceOf<CreatedResult>());
    }

    [Test]
    public async Task UpdateAgent_ReturnsOk()
    {
        var dto = new AgentUpdateDto();
        _agentServiceMock.Setup(x => x.UpdateAgent("agentUser", 1, dto)).ReturnsAsync(new Agent());

        var result = await _controller.UpdateAgent(1, dto);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task DeleteAgent_ReturnsOk()
    {
        _agentServiceMock.Setup(x => x.DeleteAgent("agentUser", 1)).ReturnsAsync(new Agent());

        var result = await _controller.DeleteAgent(1);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetAgentById_ReturnsOk()
    {
        _agentServiceMock.Setup(x => x.GetAgentById(1)).ReturnsAsync(new Agent());

        var result = await _controller.GetAgentById(1);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetAgents_ReturnsOk()
    {
        _agentServiceMock.Setup(x => x.GetAgents(It.IsAny<AgentQueryParams>())).ReturnsAsync(new List<Agent>());

        var result = await _controller.GetAgents(new AgentQueryParams());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
}
