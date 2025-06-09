using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
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
            var chatMessage = await _chatMessageService.CreateTextMessage(chatId, chatMessageDto);
            return Ok(chatMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] int chatId, int id, [FromBody] ChatMessageEditDto chatMessageDto)
        {
            var message = await _chatMessageService.EditMessage(chatId, id, chatMessageDto);
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int chatId, int id)
        {
            var chatMessage = await _chatMessageService.DeleteMessage(chatId, id);
            return Ok(chatMessage);
        }

        [HttpGet]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> GetMessages([FromRoute] int chatId)
        {
            var messages = await _chatMessageService.GetMessages(chatId);
            return Ok(messages);
        }
    }
}
