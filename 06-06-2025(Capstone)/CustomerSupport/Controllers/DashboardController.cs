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
            var closedChat = await _dashboardService.GetClosedChatCount();
            var agent = await _dashboardService.GetAgentCount();
            var customer = await _dashboardService.GetCustomerCount();
            return Ok(new
            {
                activeChat,
                chatCount,
                closedChat, 
                agent,
                customer
            });
        }

        [HttpGet("userSummary")]
        [Authorize(Roles = "Customer, Agent")]
        public async Task<IActionResult> UserDashboardSummary()
        {
            var user = User?.Identity?.Name;
            var activeChat = await _dashboardService.GetActiveChatCountByUser(user ?? "");
            var chatCount = await _dashboardService.GetChatCountByUser(user ?? "");
            var closedChat = await _dashboardService.GetClosedChatCountByUser(user ?? "");
            return Ok(new
            {
                activeChat,
                chatCount,
                closedChat
            });
        }

        [HttpGet("chatTrend")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChatTrend()
        {
            var trends = await _dashboardService.GetAdminChatTrend();
            return Ok(trends);
        }

        [HttpGet("userChatTrend")]
        [Authorize(Roles = "Customer, Agent")]
        public async Task<IActionResult> UserChatTrend()
        {
            var user = User?.Identity?.Name;
            var trends = await _dashboardService.GetUserChatTrend(user ?? "");
            return Ok(trends);
        }

    }
}