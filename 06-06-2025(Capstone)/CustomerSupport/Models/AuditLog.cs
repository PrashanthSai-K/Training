using System;

namespace CustomerSupport.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    
    public User? User { get; set; }
}
