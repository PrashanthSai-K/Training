using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models.Dto;

public class ChatMessageCreateDto
{
    public string UserId { get; set; } = string.Empty;
    [Required]
    public string? Message { get; set; }

}
