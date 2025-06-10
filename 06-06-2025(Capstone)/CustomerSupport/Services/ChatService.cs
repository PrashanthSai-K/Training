using System;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;

namespace CustomerSupport.Services;

public class ChatService : IChatService
{
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<int, Agent> _agentRepository;
    private readonly IRepository<int, Customer> _customerRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly IMapper _mapper;

    public ChatService(IRepository<int, Chat> chatRepository,
                        IRepository<int, Agent> agentRepository,
                        IRepository<int, Customer> customerRepository,
                        IAuditLogService auditLogService,
                        IMapper mapper)
    {
        _chatRepository = chatRepository;
        _agentRepository = agentRepository;
        _customerRepository = customerRepository;
        _auditLogService = auditLogService;
        _mapper = mapper;
    }

    public async Task<Chat> CreateChat(string userId, ChatCreationDto chatDto)
    {
        var agent = await _agentRepository.GetById(chatDto.AgentId);
        var customer = await _customerRepository.GetById(chatDto.CustomerId);

        var chat = _mapper.Map<ChatCreationDto, Chat>(chatDto);
        chat.CreatedOn = DateTime.UtcNow;
        chat.Status = "Active";

        var createdChat = await _chatRepository.Create(chat);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Create", Entity = "Chat", CreatedOn = DateTime.UtcNow });

        return createdChat;
    }

    public async Task<Chat> DeleteChat(string userId, int id)
    {
        var chat = await _chatRepository.GetById(id);
        var agents = await _agentRepository.GetAll();

        var agent = agents.FirstOrDefault(a => a.Email == userId);
        if (agent == null || chat.AgentId != agent.Id)
            throw new UnauthorizedAccessException("User not allowed to delete this chat");

        chat.Status = "Deleted";
        var deletedChat = await _chatRepository.Update(id, chat);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Delete", Entity = "Chat", CreatedOn = DateTime.UtcNow });

        return deletedChat;
    }

    public async Task<Chat> GetChatById(int id)
    {
        var chat = await _chatRepository.GetById(id);
        return chat;
    }

    public async Task<IEnumerable<Chat>> GetChats(ChatQueryParams queryParams)
    {
        var chats = await _chatRepository.GetAll();

        chats = GetChatsByAgent(chats, queryParams.AgentId);
        chats = GetChatsByCustomer(chats, queryParams.CustomerId);
        chats = GetChatsByStatus(chats, queryParams.Status);
        chats = GetChatsByDate(chats, queryParams.ChatCreatedDate);

        chats = chats.Skip((queryParams.PageNumber - 1) * queryParams.PageSize).Take(queryParams.PageSize);

        return chats;
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
