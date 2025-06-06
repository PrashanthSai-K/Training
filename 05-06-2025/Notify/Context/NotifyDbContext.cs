using System;
using Microsoft.EntityFrameworkCore;
using Notify.Models;

namespace Notify.Context;

public class NotifyDbContext : DbContext
{
    public NotifyDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Upload> Uploads { get; set; }
    
}
