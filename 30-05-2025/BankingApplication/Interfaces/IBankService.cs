using System;
using BankingApplication.Models;
using BankingApplication.Models.DTO;

namespace BankingApplication.Interfaces;

public interface IBankService
{
    Task<Account> CreateAccount(Account account);
    Task<double> GetAccountBalance(int k);
    Task<Account> Withdraw(WithdrawDto withdrawDto);
    Task<Account> Deposit(DepositDto depositDto);
    Task<TransferDto> Transfer(TransferDto transferDto);
}
