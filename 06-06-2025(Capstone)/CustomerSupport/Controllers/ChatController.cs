using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        [Authorize(Roles = "Agent, Customer")]
        public async Task<IActionResult> CreateChat(ChatCreationDto chatDto)
        {
            var userId = User?.Identity?.Name ?? "";

            var chat = await _chatService.CreateChat(userId, chatDto);
            return Ok(chat);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            var userId = User?.Identity?.Name ?? "";

            var chat = await _chatService.DeleteChat(userId, id);
            return Ok(chat);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetChatById(int id)
        {
            var chat = await _chatService.GetChatById(id);
            return Ok(chat);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetChats([FromQuery] ChatQueryParams queryParams)
        {
            var chats = await _chatService.GetChats(queryParams);
            return Ok(chats);
        }


    }
}
