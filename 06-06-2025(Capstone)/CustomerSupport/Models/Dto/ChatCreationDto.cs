using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models.Dto;

public class ChatCreationDto
{
    [Required]
    public string IssueName { get; set; } = string.Empty;
    [Required]
    public string IssueDescription { get; set; } = string.Empty;
}
