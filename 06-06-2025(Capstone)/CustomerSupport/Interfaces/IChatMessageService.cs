using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IChatMessageService
{
    public Task<ChatMessage> CreateTextMessage(int chatId, ChatMessageCreateDto chatMessageDto);
    public Task<ChatMessage> EditMessage(int chatId, int id, ChatMessageEditDto chatMessageDto);
    public Task<ChatMessage> DeleteMessage(int chatId, int id);
    public Task<IEnumerable<ChatMessage>> GetMessages(int chatId);
}
