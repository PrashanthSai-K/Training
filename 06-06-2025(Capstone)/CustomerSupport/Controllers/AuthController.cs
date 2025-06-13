using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CustomerSupport.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [EnableRateLimiting("RateLimiter")]
        public async Task<IActionResult> LoginUser(LoginRequestDto requestDto)
        {
            var result = await _authService.AuthenticateUser(requestDto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshSession(RefreshSessionRequestDto requestDto)
        {
            var result = await _authService.RefreshUserSession(requestDto);
            return Ok(result);
        }
    }
}
