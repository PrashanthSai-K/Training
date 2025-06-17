using System;
using CustomerSupport.Exceptions;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<string, User> _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IHashingService _hashingService;

    public AuthService(IRepository<string, User> userRepository, ITokenService tokenService, IHashingService hashingService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _hashingService = hashingService;
    }
    public async Task<LoginResponseDto> AuthenticateUser(LoginRequestDto requestDto)
    {
        var user = await _userRepository.GetById(requestDto.Username);

        if (user.Status != "Active")
            throw new UnauthorizedAccessException("User account has been deactivated");
        
        if (!_hashingService.VerifyHash(requestDto.Password, user.Password))
            throw new PassowrdWrongException("Password in invalid");

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefereshToken(user);

        user.RefreshToken = refreshToken;
        await _userRepository.Update(user.Username, user);

        return new LoginResponseDto()
        {
            Username = user.Username,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponseDto> RefreshUserSession(RefreshSessionRequestDto requestDto)
    {
        var user = await _userRepository.GetById(requestDto.Username);

        if (user.RefreshToken != requestDto.RefreshToken)
            throw new InvalidTokenException("Refresh token is not valid");

        Console.WriteLine(_tokenService.CheckRefreshTokenValidity(requestDto.RefreshToken));
        if (!_tokenService.CheckRefreshTokenValidity(requestDto.RefreshToken))
            throw new InvalidTokenException("Refresh token expired");

        var accessToken = _tokenService.GenerateAccessToken(user);

        return new LoginResponseDto()
        {
            Username = user.Username,
            AccessToken = accessToken,
            RefreshToken = user.RefreshToken
        };
    }
}
