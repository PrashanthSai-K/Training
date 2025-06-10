using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;

namespace CustomerSupport.Interfaces;

public interface IChatMessageService
{
    public Task<ChatMessage> CreateTextMessage(string? userId, int chatId, ChatMessageCreateDto chatMessageDto);
    public Task<ChatMessage> EditMessage(string? userId, int chatId, int id, ChatMessageEditDto chatMessageDto);
    public Task<ChatMessage> DeleteMessage(string? userId, int chatId, int id);
    public Task<IEnumerable<ChatMessage>> GetMessages(int chatId, ChatMessageQueryParams queryParams);
}
