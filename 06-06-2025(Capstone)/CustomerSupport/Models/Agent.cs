using System;

namespace CustomerSupport.Models;

public class Agent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DateOfJoin { get; set; }

    public User? User { get; set; }
    public ICollection<Chat>? Chats { get; set; }
}
