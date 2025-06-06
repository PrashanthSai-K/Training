using System;
using Microsoft.AspNetCore.Authentication;
using Notify.Models.Dto;

namespace Notify.Interfaces;

public interface IAuthService
{
    public Task<LoginResponseDto> LoginUser(AuthenticateResult result);

}
