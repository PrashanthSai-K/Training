using System;

namespace BankingApplication.Models.DTO;

public class WithdrawDto
{
    public int AccountID { get; set; }
    public double WithdrawAmount { get; set; }
    
}
