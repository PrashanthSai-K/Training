using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models.Dto;

public class RefreshSessionRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
