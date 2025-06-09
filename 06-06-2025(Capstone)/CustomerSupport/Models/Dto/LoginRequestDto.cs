using System;
using System.ComponentModel.DataAnnotations;
using CustomerSupport.Validation;


namespace CustomerSupport.Models.Dto;

public class LoginRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [PasswordValidator]
    public string Password { get; set; } = string.Empty;
}
