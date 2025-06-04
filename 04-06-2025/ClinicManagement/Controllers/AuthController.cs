using ClinicManagement.Interfaces;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Misc;


namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService;
        }

        [HttpPost]
        [CustomerExceptionFilter]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto login)
        {
            var token = await _authService.Login(login);
            return Ok(token);
        }
    }
}
