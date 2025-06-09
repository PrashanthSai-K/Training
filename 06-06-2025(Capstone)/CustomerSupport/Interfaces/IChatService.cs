using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IChatService
{
    public Task<Chat> CreateChat(ChatCreationDto chatDto);
    public Task<Chat> DeleteChat(int id);
    public Task<Chat> GetChatById(int id);
    public Task<IEnumerable<Chat>> GetChats();
}
