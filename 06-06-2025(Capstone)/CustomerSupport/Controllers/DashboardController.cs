using System.Threading.Tasks;
using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("adminSummary")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DashboardSummary()
        {
            var activeChat = await _dashboardService.GetActiveChatCount();
            var chatCount = await _dashboardService.GetChatCount();
            var agent = await _dashboardService.GetAgentCount();
            var customer = await _dashboardService.GetCustomerCount();
            return Ok(new
            {
                activeChat,
                chatCount, 
                agent,
                customer
            });
        }

        // [HttpGet("agentCount")]
        // public IActionResult AgentCount()
        // {
        //     return Ok();
        // }

        // public IActionResult CustomerCount()
        // {
        //     return Ok();
        // }

        // public IActionResult ChatCountByAgent(ChatCountRequestDto requestDto)
        // {
        //     return Ok();
        // }

    }
}