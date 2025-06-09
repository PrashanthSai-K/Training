using System;
using ClinicManagement.Repository;
using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class UserRepository : Repository<string, User>
{
    public UserRepository(ChatDbContext chatDbContext) : base(chatDbContext)
    {
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        var users = await _chatDbContext.Users.ToListAsync()
                ?? throw new EntityEmptyException("No Users found in database");
        return users;
    }

    public override async Task<User> GetById(string username)
    {
        var customer = await _chatDbContext.Users.FirstOrDefaultAsync(a => a.Username == username)
                        ?? throw new ItemNotFoundException($"User with username : {username} not found");
        return customer;
    }
}
