using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models;

public class User
{
    [Key]
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Roles { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public Agent? Agent { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<ChatMessage>? ChatMessages { get; set; }
    public ICollection<AuditLog>? AuditLogs{ get; set; }
}
