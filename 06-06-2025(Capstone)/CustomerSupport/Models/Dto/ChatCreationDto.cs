using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models.Dto;

public class ChatCreationDto
{
    [Required]
    public int AgentId { get; set; }
    [Required]
    public int CustomerId { get; set; }
}
