using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models.Dto;

public class ImageUploadDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    public required IFormFile formFile{ get; set; }
}
