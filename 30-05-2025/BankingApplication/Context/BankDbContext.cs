using System;
using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Context;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    
}
