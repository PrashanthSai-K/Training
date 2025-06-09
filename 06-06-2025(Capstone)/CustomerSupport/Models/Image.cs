using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models;

public class Image
{
    [Key]
    public string ImageName { get; set; } = string.Empty;
    public byte[] ImageData { get; set; } = new byte[0];
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public ChatMessage? ChatMessages{ get; set; }
}
