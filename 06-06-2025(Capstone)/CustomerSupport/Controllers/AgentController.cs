using System.Security.Claims;
using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CustomerSupport.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAgent(AgentRegisterDto agentDto)
        {
            var agent = await _agentService.CreateAgent(agentDto);
            return Created("", agent);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Agent, Admin")]
        public async Task<IActionResult> UpdateAgent(int id, AgentUpdateDto agentDto)
        {
            var userId = User?.Identity?.Name;
            var updatedAgent = await _agentService.UpdateAgent(userId, id, agentDto);
            return Ok(updatedAgent);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Agent, Admin")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            var userId = User?.Identity?.Name;

            var updatedAgent = await _agentService.DeleteAgent(userId, id);
            return Ok(updatedAgent);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAgentById(int id)
        {
            var agent = await _agentService.GetAgentById(id);
            return Ok(agent);
        }

        [HttpGet]
        [Authorize]
        [EnableRateLimiting("RateLimiter")]
        public async Task<IActionResult> GetAgents([FromQuery] AgentQueryParams queryParams)
        {
            var agents = await _agentService.GetAgents(queryParams);
            return Ok(agents);
        }

        [HttpPut("{id}/activate")]
        [Authorize(Roles = "Agent, Admin")]
        public async Task<IActionResult> ActivateAgent(int id)
        {
            var userId = User?.Identity?.Name;

            var agent = await _agentService.ActivateAgent(userId, id);
            return Ok(agent);
        }

        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "Agent, Admin")]
        public async Task<IActionResult> DeactivateAgent(int id)
        {
            var userId = User?.Identity?.Name;
            var agent = await _agentService.DeactivateAgent(userId, id);
            return Ok(agent);
        }

    }
}
