using System;
using Microsoft.EntityFrameworkCore;
using Notify.Context;
using Notify.Models;

namespace Notify.Repository;

public class UserRepository : Repository<int, User>
{
    public UserRepository(NotifyDbContext context) : base(context)
    {
    }
    
    public override async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync() ?? throw new ArgumentNullException("No users found in database");
    }

    public override Task<User> GetById(int id)
    {
        throw new NotImplementedException();
    }
}
