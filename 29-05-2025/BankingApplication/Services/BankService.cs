using System;
using BankingApplication.Context;
using BankingApplication.Interfaces;
using BankingApplication.Models;
using BankingApplication.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Services;

public class BankService : IBankService
{
    private readonly IRepository<int, Account> _bankRepository;
    private readonly BankDbContext _bankDbContext;

    public BankService(IRepository<int, Account> bankRepository, BankDbContext bankDbContext)
    {
        _bankRepository = bankRepository;
        _bankDbContext = bankDbContext;
    }

    public async Task<Account> CreateAccount(Account account)
    {
        account.Status = "ACTIVE";
        await _bankRepository.Create(account);
        return account;
    }

    public async Task<Account> Deposit(DepositDto depositDto)
    {
        var account = await _bankRepository.GetById(depositDto.AccountID);
        account.Balance += depositDto.DepositAmount;
        await _bankRepository.Update(depositDto.AccountID, account);
        return account;
    }

    public async Task<Account> Withdraw(WithdrawDto withdrawDto)
    {
        var account = await _bankRepository.GetById(withdrawDto.AccountID);
        if (account.Balance < withdrawDto.WithdrawAmount)
            throw new Exception("Insufficient Account Balance.");
        account.Balance -= withdrawDto.WithdrawAmount;
        await _bankRepository.Update(withdrawDto.AccountID, account);
        return account;
    }

    public async Task<double> GetAccountBalance(int k)
    {
        var account = await _bankRepository.GetById(k);
        return account.Balance;
    }

    public async Task<TransferDto> Transfer(TransferDto transferDto)
    {
        var DebitAccount = await _bankDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == transferDto.DebitAccountID)
                    ?? throw new Exception($"Debit Account with ID {transferDto.DebitAccountID} not found");

        var CreditAccount = await _bankDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == transferDto.CreditAccountID)
                            ?? throw new Exception($"Credit Account with ID {transferDto.CreditAccountID} not found");

        if (DebitAccount.Balance < transferDto.TransferAmount)
            throw new Exception("Insufficient Account Balance.");

        using var transaction = await _bankDbContext.Database.BeginTransactionAsync();
        try
        {
            DebitAccount.Balance -= transferDto.TransferAmount;
            CreditAccount.Balance += transferDto.TransferAmount;
            _bankDbContext.Accounts.Update(DebitAccount);
            _bankDbContext.Accounts.Update(CreditAccount);
            await _bankDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return transferDto;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception($"{e.Message}");
        }
    }


}
