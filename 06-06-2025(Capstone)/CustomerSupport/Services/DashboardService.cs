using System;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Services;

public class DashboardService : IDashboardService
{
    private readonly IRepository<int, Agent> _agentRepository;
    private readonly IRepository<int, Customer> _customerRepository;
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<string, User> _userRepository;

    public DashboardService(IRepository<int, Agent> agentRepository,
                            IRepository<int, Customer> customerRepository,
                            IRepository<int, Chat> chatRepository,
                            IRepository<string, User> userRepository)
    {
        _agentRepository = agentRepository;
        _customerRepository = customerRepository;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }
    public async Task<int> GetAgentCount()
    {
        var agents = await _agentRepository.GetAll();
        return agents.Count();
    }

    public async Task<int> GetChatCount()
    {
        var chats = await _chatRepository.GetAll();
        return chats.Count();
    }

    public async Task<int> GetActiveChatCount()
    {
        var chats = await _chatRepository.GetAll();
        chats = chats.Where(chat => chat.Status == "Active").ToList();

        return chats.Count();
    }
    public async Task<int> GetClosedChatCount()
    {
        var chats = await _chatRepository.GetAll();
        chats = chats.Where(chat => chat.Status == "Deleted").ToList();
        return chats.Count();
    }

    public async Task<int> GetChatCountByAgent(string userId, ChatCountRequestDto requestDto)
    {
        var user = await _userRepository.GetById(userId);
        var agents = await _agentRepository.GetAll();
        var agent = agents.FirstOrDefault(agent => agent.Email == user.Username);
        if (agent == null)
            throw new Exception("Agent not found");

        var chats = await _chatRepository.GetAll();
        chats = chats.Where(chat => chat.Status == requestDto.Status && chat.AgentId == agent.Id).ToList();
        return chats.Count();
    }

    public async Task<int> GetCustomerCount()
    {
        var customers = await _customerRepository.GetAll();
        return customers.Count();
    }
}
