using System;

namespace BankingApplication.Models;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public double Balance { get; set; }
    public string Status { get; set; } = String.Empty;

}
