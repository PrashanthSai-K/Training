using System;

namespace CustomerSupport.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;


    public User? User { get; set; }
    public ICollection<Chat>? Chats { get; set; }
}
