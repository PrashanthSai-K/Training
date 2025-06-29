using System;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IDashboardService
{
    public Task<int> GetChatCount();
    public Task<int> GetActiveChatCount();
    public Task<int> GetClosedChatCount();
    public Task<int> GetChatCountByAgent(string userId, ChatCountRequestDto requestDto);
    public Task<int> GetAgentCount();
    public Task<int> GetCustomerCount();

}
