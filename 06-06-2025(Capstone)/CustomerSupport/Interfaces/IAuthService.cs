using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IAuthService
{
    public Task<LoginResponseDto> AuthenticateUser(LoginRequestDto requestDto);
    public Task<LoginResponseDto> RefreshUserSession(RefreshSessionRequestDto requestDto);
    public Task<string> ForgotPassword(ForgotPasswordRequestDto requestDto);
    public Task<User> ResetPassword(ResetPasswordRequestDto requestDto);

}
