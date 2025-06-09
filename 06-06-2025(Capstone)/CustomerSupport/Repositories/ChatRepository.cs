using System;
using ClinicManagement.Repository;
using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class ChatRepository : Repository<int, Chat>
{
    public ChatRepository(ChatDbContext chatDbContext) : base(chatDbContext)
    {
    }

    public override async Task<IEnumerable<Chat>> GetAll()
    {
        var chats = await _chatDbContext.Chats.ToListAsync()
                ?? throw new EntityEmptyException("No Chats found in database");
        return chats;
    }

    public override async Task<Chat> GetById(int id)
    {
        var chat = await _chatDbContext.Chats.FirstOrDefaultAsync(a => a.Id == id)
                        ?? throw new ItemNotFoundException($"Chat with Id : {id} not found");
        return chat;
    }
}
