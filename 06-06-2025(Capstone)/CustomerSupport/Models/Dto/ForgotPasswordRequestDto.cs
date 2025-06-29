using System;

namespace CustomerSupport.Models.Dto;

public class ForgotPasswordRequestDto
{
    public string Email { get; set; } = string.Empty;
}
