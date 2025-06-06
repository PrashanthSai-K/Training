using System;

namespace Notify.Models.Dto;

public class LoginResponseDto
{
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
