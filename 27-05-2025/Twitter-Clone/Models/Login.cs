using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter_Clone.Models;

public class Login
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
