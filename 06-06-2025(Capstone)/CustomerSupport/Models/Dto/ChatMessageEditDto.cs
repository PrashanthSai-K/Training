using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models.Dto;

public class ChatMessageEditDto
{
    [Required]
    public string? Message { get; set; }

}
