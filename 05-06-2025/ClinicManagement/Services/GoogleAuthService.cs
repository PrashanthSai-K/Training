using System;
using System.Security.Claims;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Authentication;

namespace ClinicManagement.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly ITokenService _tokenService;

    public GoogleAuthService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> AuthenticateUser(AuthenticateResult result)
    {
        var name = result.Principal?.FindFirst(ClaimTypes.Name)?.Value;
        var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;


        var user = new User()
        {
            UserName = email,
            Role = "Doctor"
        };

        var token = await _tokenService.GenerateToken(user, 0);

        return new LoginResponseDto()
        {
            Username = name,
            Token = token
        };

    }
}
