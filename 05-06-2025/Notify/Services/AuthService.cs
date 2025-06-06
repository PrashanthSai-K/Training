using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Notify.Interfaces;
using Notify.Models;
using Notify.Models.Dto;

namespace Notify.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<int, User> _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IRepository<int, User> userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> LoginUser(AuthenticateResult result)
    {
        var email = result?.Principal?.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
            throw new UnauthorizedAccessException("Not authorized");

        var users = await _userRepository.GetAll();
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null)
            throw new UnauthorizedAccessException("Not authorized");

        var token = await _tokenService.GenerateToken(user);

        return new LoginResponseDto()
        {
            Username = user.Email,
            Token = token
        };
    }
}
