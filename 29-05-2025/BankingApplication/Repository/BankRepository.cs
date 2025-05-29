using System;
using BankingApplication.Context;
using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Repository;

public class BankRepository : Repository<int, Account>
{
    public BankRepository(BankDbContext bankDbContext) : base(bankDbContext)
    {
    }
    
    public override async Task<IEnumerable<Account>> GetAll()
    {
        return await _bankDbContext.Accounts.ToListAsync() ?? throw new Exception("No Accounts found in Bank");
    }

    public override async Task<Account> GetById(int id)
    {
        return await _bankDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new Exception($"No Accounts with Id : {id} found in Bank");
    }
}
