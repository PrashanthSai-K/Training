using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models;

public class User
{
    [Key]
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public byte[]? Password { get; set; }
    public byte[]? HashKey { get; set; }

    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }

}
