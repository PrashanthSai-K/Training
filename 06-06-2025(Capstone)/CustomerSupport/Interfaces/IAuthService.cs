using System;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IAuthService
{
    public Task<LoginResponseDto> AuthenticateUser(LoginRequestDto requestDto);
    public Task<LoginResponseDto> RefreshUserSession(RefreshSessionRequestDto requestDto);
}
