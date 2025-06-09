using System;

namespace CustomerSupport.Models;

public class Chat
{
    public int Id { get; set; }
    public int AgentId { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public Agent? Agent{ get; set; }
    public Customer? Customer{ get; set; }
    public ICollection<ChatMessage>? ChatMessages { get; set; }
}
