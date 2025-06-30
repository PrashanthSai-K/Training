using System;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IDashboardService
{
    public Task<int> GetChatCount();
    public Task<int> GetActiveChatCount();
    public Task<int> GetClosedChatCount();
    public Task<int> GetChatCountByUser(string userId);
    public Task<int> GetActiveChatCountByUser(string userId);
    public Task<int> GetClosedChatCountByUser(string userId);
    public Task<int> GetAgentCount();
    public Task<int> GetCustomerCount();
    public Task<IEnumerable<AdminChatTrendDto>> GetAdminChatTrend();
    public Task<IEnumerable<AdminChatTrendDto>> GetUserChatTrend(string userId);
}
