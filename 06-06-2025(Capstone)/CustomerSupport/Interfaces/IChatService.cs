using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;

namespace CustomerSupport.Interfaces;

public interface IChatService
{
    public Task<Chat> CreateChat(string userId, ChatCreationDto chatDto);
    public Task<Chat> DeleteChat(string userId, int id);
    public Task<Chat> GetChatById(int id);
    public Task<IEnumerable<Chat>> GetChats(string userId, ChatQueryParams queryParams);
}
