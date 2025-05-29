using System;

namespace BankingApplication.Models.DTO;

public class AccountCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Double Balance { get; set; }

}
