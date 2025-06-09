using System;
using ClinicManagement.Repository;
using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class ChatMessagesRepository : Repository<int, ChatMessage>
{
    public ChatMessagesRepository(ChatDbContext chatDbContext) : base(chatDbContext)
    {
    }

    public override async Task<IEnumerable<ChatMessage>> GetAll()
    {
        var messages = await _chatDbContext.ChatMessages.ToListAsync()
                ?? throw new EntityEmptyException("No Chat messages found in database");
        return messages;
    }

    public override async Task<ChatMessage> GetById(int id)
    {
        var message = await _chatDbContext.ChatMessages.FirstOrDefaultAsync(a => a.Id == id)
                        ?? throw new ItemNotFoundException($"Chat message with Id : {id} not found");
        return message;
    }
}
