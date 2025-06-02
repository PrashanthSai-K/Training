using System;

namespace BankingApplication.Models.DTO;

public class TransferDto
{
    public int DebitAccountID { get; set; }
    public int CreditAccountID { get; set; }
    public double TransferAmount { get; set; }
    
}
