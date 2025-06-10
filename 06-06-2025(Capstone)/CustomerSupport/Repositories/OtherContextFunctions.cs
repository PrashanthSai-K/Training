using System;
using CustomerSupport.Context;
using CustomerSupport.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class OtherContextFunctions : IOtherContextFunctions
{
    private readonly ChatDbContext _chatDbContext;

    public OtherContextFunctions(ChatDbContext chatDbContext)
    {
        _chatDbContext = chatDbContext;
    }
    public async Task<bool> IsUserInChat(int chatId, string email)
    {
        return await _chatDbContext.Chats
                             .Where(c => c.Id == chatId)
                             .AnyAsync(c => (c.Agent != null && c.Agent.Email == email)
                             || (c.Customer != null && c.Customer.Email == email));
    }
}
