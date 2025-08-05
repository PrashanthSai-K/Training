using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
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
    private readonly IEmailService _emailService;

    public AuthService(IRepository<string, User> userRepository, ITokenService tokenService, IHashingService hashingService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _hashingService = hashingService;
        _emailService = emailService;
    }
    public async Task<LoginResponseDto> AuthenticateUser(LoginRequestDto requestDto)
    {
        var user = await _userRepository.GetById(requestDto.Username);

        if (user.Roles == "Customer" && user.Status != "Active")
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

    public async Task<string> ForgotPassword(ForgotPasswordRequestDto requestDto)
    {
        var user = await _userRepository.GetById(requestDto.Email);
        var token = _tokenService.GenerateRefereshToken(user);

        user.RefreshToken = token;
        await _userRepository.Update(user.Username, user);

        if (!_emailService.SendEmail(user.Username, token))
            throw new Exception("Mail not sent");

        return token;
    }

    public async Task<User> ResetPassword(ResetPasswordRequestDto requestDto)
    {        
        var user = await _userRepository.GetById(requestDto.Email);

        if (user.RefreshToken != requestDto.Token)
            throw new InvalidTokenException("Refresh token is not valid");

        if (!_tokenService.CheckRefreshTokenValidity(requestDto.Token))
            throw new InvalidTokenException("Password token expired");

        var password = _hashingService.HashData(requestDto.Password);
        user.Password = password;
        var updatedUser = await _userRepository.Update(user.Username, user);
        return updatedUser;
    }

    public async Task<LoginResponseDto> RefreshUserSession(RefreshSessionRequestDto requestDto)
    {
        var user = await _userRepository.GetById(requestDto.Username);

        if (user.RefreshToken != requestDto.RefreshToken)
            throw new InvalidTokenException("Refresh token is not valid");

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
