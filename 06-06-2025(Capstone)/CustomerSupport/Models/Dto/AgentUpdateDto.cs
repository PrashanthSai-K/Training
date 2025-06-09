using System;
using System.ComponentModel.DataAnnotations;
using CustomerSupport.Validation;


namespace CustomerSupport.Models.Dto;

public class AgentUpdateDto
{
    [NameValidation]
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfJoin { get; set; }
}
