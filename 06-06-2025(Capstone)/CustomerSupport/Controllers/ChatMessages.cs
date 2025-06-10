using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
    [Route("api/v1/chat/{chatId}/message")]
    [ApiController]
    public class ChatMessages : ControllerBase
    {
        private readonly IChatMessageService _chatMessageService;

        public ChatMessages(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }

        [HttpPost]
        [Authorize(Roles = "Agent, Customer")]
        public async Task<IActionResult> CreateMessage([FromRoute] int chatId, [FromBody] ChatMessageCreateDto chatMessageDto)
        {
            var userId = User.Identity?.Name;

            var chatMessage = await _chatMessageService.CreateTextMessage(userId, chatId, chatMessageDto);
            return Ok(chatMessage);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Agent, Customer")]
        public async Task<IActionResult> UpdateMessage([FromRoute] int chatId, int id, [FromBody] ChatMessageEditDto chatMessageDto)
        {
            var userId = User.Identity?.Name;

            var message = await _chatMessageService.EditMessage(userId, chatId, id, chatMessageDto);
            return Ok(message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Agent, Customer")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int chatId, int id)
        {
            var userId = User.Identity?.Name;

            var chatMessage = await _chatMessageService.DeleteMessage(userId, chatId, id);
            return Ok(chatMessage);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMessages([FromRoute] int chatId, [FromQuery]ChatMessageQueryParams queryParams)
        {
            var messages = await _chatMessageService.GetMessages(chatId, queryParams);
            return Ok(messages);
        }
    }
}
