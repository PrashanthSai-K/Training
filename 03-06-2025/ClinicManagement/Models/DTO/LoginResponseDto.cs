using System;

namespace ClinicManagement.Models.DTO;

public class LoginResponseDto
{
    public string Username { get; set; } = string.Empty;
    public string? Token { get; set; }

}
