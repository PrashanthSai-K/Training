using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomerSupport.Exceptions;
using CustomerSupport.Interfaces;
using CustomerSupport.MessageHub;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.SignalR;

namespace CustomerSupport.Services;

public class ChatService : IChatService
{
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<int, Agent> _agentRepository;
    private readonly IRepository<int, Customer> _customerRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly IOtherContextFunctions _otherContextFunctions;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly IMapper _mapper;

    public ChatService(IRepository<int, Chat> chatRepository,
                        IRepository<int, Agent> agentRepository,
                        IRepository<int, Customer> customerRepository,
                        IAuditLogService auditLogService,
                        IOtherContextFunctions otherContextFunctions,
                        IHubContext<ChatHub> hubContext,
                        IMapper mapper)
    {
        _chatRepository = chatRepository;
        _agentRepository = agentRepository;
        _customerRepository = customerRepository;
        _auditLogService = auditLogService;
        _otherContextFunctions = otherContextFunctions;
        _chatHub = hubContext;
        _mapper = mapper;
    }

    public async Task<Chat> CreateChat(string userId, ChatCreationDto chatDto)
    {
        var customers = await _customerRepository.GetAll();
        var customer = customers.FirstOrDefault(c => c.Email == userId);
        if (customer == null)
            throw new ItemNotFoundException($"User with UserId:{userId} not found");

        var agents = await _agentRepository.GetAll();
        agents = agents.Where(agent => agent.Status == "Active").ToList();
        
        if (agents == null || !agents.Any())
            throw new Exception("No agents available for assignment.");

        var random = new Random();
        var randomAgent = agents.ElementAt(random.Next(agents.Count()));

        var chat = _mapper.Map<ChatCreationDto, Chat>(chatDto);
        chat.CustomerId = customer.Id;
        chat.AgentId = randomAgent.Id;
        chat.CreatedOn = DateTime.UtcNow;
        chat.UpdatedAt = DateTime.UtcNow;
        chat.Status = "Active";

        var createdChat = await _chatRepository.Create(chat);
        await _chatHub.Clients.All.SendAsync("ReceiveAssignedNotification", new
        {
            chatId = chat.Id,
            chat.AgentId,
            chat.CustomerId,
            chat.IssueName,
            chat.CreatedOn,
            status = chat.Status,
            updatedAt = chat.UpdatedAt,
            customerEmail = customer.Email,
            agentEmail = randomAgent.Email
        });

        Console.WriteLine($"Sending to groups: {customer.Email}, {randomAgent.Email}");

        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Create", Entity = "Chat", CreatedOn = DateTime.UtcNow });

        return createdChat;
    }

    public async Task<Chat> DeleteChat(string userId, int id)
    {
        var chat = await _chatRepository.GetById(id);
        var agents = await _agentRepository.GetAll();
        var customers = await _customerRepository.GetAll();
        var customer = customers.FirstOrDefault(customer => customer.Id == chat.CustomerId);

        var agent = agents.FirstOrDefault(a => a.Email == userId);
        if (agent == null || customer == null || chat.AgentId != agent.Id)
            throw new UnauthorizedAccessException("User not allowed to delete this chat");

        chat.UpdatedAt = DateTime.UtcNow;
        chat.Status = "Deleted";
        var deletedChat = await _chatRepository.Update(id, chat);
        Console.WriteLine("created cat");
        await _chatHub.Clients.Group(chat.Id.ToString()).SendAsync("ReceiveClosedNotification", new
        {
            chatId = chat.Id,
            chat.AgentId,
            chat.CustomerId,
            chat.IssueName,
            chat.CreatedOn,
            status = chat.Status,
            updatedAt = chat.UpdatedAt,
            customerEmail = customer.Email,
            agentEmail = agent.Email
        });

        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Delete", Entity = "Chat", CreatedOn = DateTime.UtcNow });

        return deletedChat;
    }

    public async Task<Chat> GetChatById(int id)
    {
        var chat = await _chatRepository.GetById(id);
        return chat;
    }

    public async Task<IEnumerable<ChatResponseDto>> GetChats(string userId, ChatQueryParams queryParams)
    {
        var chats = await _chatRepository.GetAll();
        chats = await GetChatsByUser(chats, userId);
        var customers = await _customerRepository.GetAll();
        var agents = await _agentRepository.GetAll();

        chats = chats.OrderByDescending(c => c.UpdatedAt);
        chats = GetChatByQuery(chats, queryParams.Query);
        chats = GetChatsByAgent(chats, queryParams.AgentId);
        chats = GetChatsByCustomer(chats, queryParams.CustomerId);
        chats = GetChatsByStatus(chats, queryParams.Status);
        chats = GetChatsByDate(chats, queryParams.ChatCreatedDate);

        chats = chats.Skip((queryParams.PageNumber - 1) * queryParams.PageSize).Take(queryParams.PageSize);

        return _mapper.Map<IEnumerable<ChatResponseDto>>(chats);
    }

    public IEnumerable<Chat> GetChatByQuery(IEnumerable<Chat> chats, string? query)
    {
        if (query == null || query == "" || chats.Count() == 0)
        {
            return chats;
        }

        return chats.Where(c => c.IssueName.Contains(query, StringComparison.OrdinalIgnoreCase)
                || c.IssueDescription.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<IEnumerable<Chat>> GetChatsByUser(IEnumerable<Chat> chats, string userId)
    {
        var result = new List<Chat>();

        foreach (var chat in chats)
        {
            if (await _otherContextFunctions.IsUserInChat(chat.Id, userId))
            {
                result.Add(chat);
            }
        }

        return result;
    }

    public IEnumerable<Chat> GetChatsByAgent(IEnumerable<Chat> chats, int? agentId)
    {
        if ((agentId ?? 0) == 0 || chats.Count() == 0)
            return chats;
        chats = chats.Where(chat => chat.AgentId == agentId);
        return chats;
    }

    public IEnumerable<Chat> GetChatsByCustomer(IEnumerable<Chat> chats, int? customerId)
    {
        if ((customerId ?? 0) == 0 || chats.Count() == 0)
            return chats;
        chats = chats.Where(chat => chat.CustomerId == customerId);
        return chats;
    }

    public IEnumerable<Chat> GetChatsByStatus(IEnumerable<Chat> chats, string? status)
    {
        if (status == null || status.Length == 0 || chats.Count() == 0)
            return chats;
        chats = chats.Where(chat => chat.Status.ToLower() == status.ToLower());
        return chats;
    }

    public IEnumerable<Chat> GetChatsByDate(IEnumerable<Chat> chats, DateRange? date)
    {
        if (date == null || chats.Count() == 0)
            return chats;

        if (date?.From != null && date?.To != null && date.From > date.To)
            return chats;

        if (date?.From != null)
            chats = chats.Where(chat => chat.CreatedOn.Date >= date.From.Value.Date);

        if (date?.To != null)
            chats = chats.Where(chat => chat.CreatedOn.Date <= date.To.Value.Date);

        return chats;
    }
}
