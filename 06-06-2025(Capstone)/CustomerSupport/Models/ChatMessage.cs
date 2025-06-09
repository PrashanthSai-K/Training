using System;

namespace CustomerSupport.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public MessageType MessageType { get; set; }
    public string? Message { get; set; }
    public string? ImageName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Image? Image{ get; set; }
    public Chat? Chat { get; set; }
    public User? User { get; set; }

}

public enum MessageType
{
    Message,
    Image
}
