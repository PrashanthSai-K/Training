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

    public async Task<int> GetChatCountByUser(string userId)
    {
        var user = await _userRepository.GetById(userId);
        IEnumerable<Chat> chats = new List<Chat>();
        if (user.Roles == "Agent")
        {
            var agents = await _agentRepository.GetAll();
            var agent = agents.FirstOrDefault(agent => agent.Email == user.Username);
            if (agent == null)
                throw new Exception("Agent not found");
            chats = await _chatRepository.GetAll();
            chats = chats.Where(chat => chat.AgentId == agent.Id).ToList();
        }
        if (user.Roles == "Customer")
        {
            var customers = await _customerRepository.GetAll();
            var customer = customers.FirstOrDefault(customer => customer.Email == user.Username);
            if (customer == null)
                throw new Exception("Customer not found");
            chats = await _chatRepository.GetAll();
            chats = chats.Where(chat => chat.CustomerId == customer.Id).ToList();
        }
        return chats.Count();
    }

    public async Task<int> GetActiveChatCountByUser(string userId)
    {
        var user = await _userRepository.GetById(userId);
        IEnumerable<Chat> chats = new List<Chat>();
        if (user.Roles == "Agent")
        {
            var agents = await _agentRepository.GetAll();
            var agent = agents.FirstOrDefault(agent => agent.Email == user.Username);
            if (agent == null)
                throw new Exception("Agent not found");
            chats = await _chatRepository.GetAll();
            chats = chats.Where(chat => chat.AgentId == agent.Id && chat.Status == "Active").ToList();
        }
        if (user.Roles == "Customer")
        {
            var customers = await _customerRepository.GetAll();
            var customer = customers.FirstOrDefault(customer => customer.Email == user.Username);
            if (customer == null)
                throw new Exception("Customer not found");
            chats = await _chatRepository.GetAll();
            chats = chats.Where(chat => chat.CustomerId == customer.Id && chat.Status == "Active").ToList();
        }
        return chats.Count();
    }

    public async Task<int> GetClosedChatCountByUser(string userId)
    {
        var user = await _userRepository.GetById(userId);
        IEnumerable<Chat> chats = new List<Chat>();
        if (user.Roles == "Agent")
        {
            var agents = await _agentRepository.GetAll();
            var agent = agents.FirstOrDefault(agent => agent.Email == user.Username);
            if (agent == null)
                throw new Exception("Agent not found");
            chats = await _chatRepository.GetAll();
            chats = chats.Where(chat => chat.AgentId == agent.Id && chat.Status == "Deleted").ToList();
        }
        if (user.Roles == "Customer")
        {
            var customers = await _customerRepository.GetAll();
            var customer = customers.FirstOrDefault(customer => customer.Email == user.Username);
            if (customer == null)
                throw new Exception("Customer not found");
            chats = await _chatRepository.GetAll();
            chats = chats.Where(chat => chat.CustomerId == customer.Id && chat.Status == "Deleted").ToList();
        }
        return chats.Count();
    }

    public async Task<int> GetCustomerCount()
    {
        var customers = await _customerRepository.GetAll();
        return customers.Count();
    }

    public async Task<IEnumerable<AdminChatTrendDto>> GetAdminChatTrend()
    {
        var chats = await _chatRepository.GetAll();
        var trends = chats.Select(chat => new AdminChatTrendDto
        {
            Status = chat.Status,
            Date = chat.Status == "Active" ? chat.CreatedOn : chat.UpdatedAt
        }).ToList();

        return trends;
    }

    public async Task<IEnumerable<AdminChatTrendDto>> GetUserChatTrend(string userId)
    {
        var user = await _userRepository.GetById(userId);
        IEnumerable<AdminChatTrendDto> trends = new List<AdminChatTrendDto>();
        if (user.Roles == "Agent")
        {
            var agents = await _agentRepository.GetAll();
            var agent = agents.FirstOrDefault(agent => agent.Email == user.Username);
            if (agent == null)
                throw new Exception("Agent not found");

            var chats = await _chatRepository.GetAll();
            trends = chats.Where(chat => chat.AgentId == agent.Id).Select(chat => new AdminChatTrendDto
            {
                Status = chat.Status,
                Date = chat.Status == "Active" ? chat.CreatedOn : chat.UpdatedAt
            }).ToList();

        }
        if (user.Roles == "Customer")
        {
            var customers = await _customerRepository.GetAll();
            var customer = customers.FirstOrDefault(customer => customer.Email == user.Username);
            if (customer == null)
                throw new Exception("Customer not found");
            var chats = await _chatRepository.GetAll();
            trends = chats.Where(chat => chat.CustomerId == customer.Id).Select(chat => new AdminChatTrendDto
            {
                Status = chat.Status,
                Date = chat.Status == "Active" ? chat.CreatedOn : chat.UpdatedAt
            }).ToList();
        }
        return trends;
    }

}
