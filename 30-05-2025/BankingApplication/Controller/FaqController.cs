using System.ComponentModel;
using System.Threading.Tasks;
using BankingApplication.Interfaces;
using BankingApplication.Models;
using BankingApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace BankingApplication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IChatService _chatService;

        public FAQController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<ActionResult> FaqChat([FromQuery] string promt)
        {
            if (string.IsNullOrWhiteSpace(promt))
                return BadRequest("Enter a proper prompt message");
            var message = await _chatService.AnswerFaq(promt);
            return Ok(message);
        }
    }

}
