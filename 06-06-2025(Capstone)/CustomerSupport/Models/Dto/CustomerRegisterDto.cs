using System;
using System.ComponentModel.DataAnnotations;
using CustomerSupport.Validation;


namespace CustomerSupport.Models.Dto;

public class CustomerRegisterDto
{
    [Required]
    [NameValidation]
    public string Name { get; set; } = string.Empty;
    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Phone { get; set; } = string.Empty;
    [Required]
    [PasswordValidator]
    public string Password { get; set; } = string.Empty;
}
