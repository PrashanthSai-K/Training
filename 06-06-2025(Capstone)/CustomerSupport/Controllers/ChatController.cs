using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(ChatCreationDto chatDto)
        {
            var chat = await _chatService.CreateChat(chatDto);
            return Ok(chat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            var chat = await _chatService.DeleteChat(id);
            return Ok(chat);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(int id)
        {
            var chat = await _chatService.GetChatById(id);
            return Ok(chat);
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var chats = await _chatService.GetChats();
            return Ok(chats);
        }


    }
}
