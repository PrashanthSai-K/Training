using System;
using System.ComponentModel.DataAnnotations;
using CustomerSupport.Validation;

namespace CustomerSupport.Models.Dto;

public class AgentRegisterDto
{
    [Required]
    [NameValidation]
    public string Name { get; set; } = string.Empty;
    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Email { get; set; } = string.Empty;
    [Required]
    public DateTime DateOfJoin { get; set; }
    [Required]
    [PasswordValidator]
    public string Password { get; set; } = string.Empty;
}
