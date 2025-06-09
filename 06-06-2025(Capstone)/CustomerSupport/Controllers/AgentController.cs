using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> UpdateAgent(int id, AgentUpdateDto agentDto)
        {
            var updatedAgent = await _agentService.UpdateAgent(id, agentDto);
            return Ok(updatedAgent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            var updatedAgent = await _agentService.DeleteAgent(id);
            return Ok(updatedAgent);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgentById(int id)
        {
            var agent = await _agentService.GetAgentById(id);
            return Ok(agent);
        }

        [HttpGet]
        public async Task<IActionResult> GetAgents([FromQuery]AgentQueryParams queryParams)
        {
            var agents = await _agentService.GetAgents(queryParams);
            return Ok(agents);
        }

    }
}
