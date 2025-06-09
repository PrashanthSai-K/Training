using System;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Services;

public class ChatService : IChatService
{
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<int, Agent> _agentRepository;
    private readonly IRepository<int, Customer> _customerRepository;
    private readonly IMapper _mapper;

    public ChatService(IRepository<int, Chat> chatRepository, IRepository<int, Agent> agentRepository, IRepository<int, Customer> customerRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _agentRepository = agentRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<Chat> CreateChat(ChatCreationDto chatDto)
    {
        var agent = await _agentRepository.GetById(chatDto.AgentId);
        var customer = await _customerRepository.GetById(chatDto.CustomerId);
        
        var chat = _mapper.Map<ChatCreationDto, Chat>(chatDto);
        chat.CreatedOn = DateTime.UtcNow;
        chat.Status = "Active";

        var createdChat = await _chatRepository.Create(chat);
        return createdChat;
    }

    public async Task<Chat> DeleteChat(int id)
    {
        var chat = await _chatRepository.GetById(id);
        chat.Status = "Deleted";
        var deletedChat = await _chatRepository.Update(id, chat);
        return deletedChat;
    }

    public async Task<Chat> GetChatById(int id)
    {
        var chat = await _chatRepository.GetById(id);
        return chat;
    }

    public async Task<IEnumerable<Chat>> GetChats()
    {
        var chats = await _chatRepository.GetAll();
        return chats;
    }
}
