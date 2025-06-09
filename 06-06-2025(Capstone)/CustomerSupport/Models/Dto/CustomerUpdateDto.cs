using System;
using CustomerSupport.Validation;


namespace CustomerSupport.Models.Dto;

public class CustomerUpdateDto
{
    [NameValidation]
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
