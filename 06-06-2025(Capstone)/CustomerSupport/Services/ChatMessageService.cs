using System;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.MessageHub;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.SignalR;

namespace CustomerSupport.Services;

public class ChatMessageService : IChatMessageService
{
    private readonly IRepository<int, ChatMessage> _chatMessageRepository;
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly IMapper _mapper;

    public ChatMessageService(IRepository<int, ChatMessage> chatMessageRepository,
                              IRepository<int, Chat> chatRepository,
                              IRepository<string, User> userRepository,
                              IHubContext<ChatHub> chatHub,
                              IMapper mapper)
    {
        _chatMessageRepository = chatMessageRepository;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _chatHub = chatHub;
        _mapper = mapper;
    }
    public async Task<ChatMessage> CreateTextMessage(int chatId, ChatMessageCreateDto chatMessageDto)
    {
        var chat = await _chatRepository.GetById(chatId);
        var user = await _userRepository.GetById(chatMessageDto.UserId);

        var chatMessage = _mapper.Map<ChatMessageCreateDto, ChatMessage>(chatMessageDto);
        chatMessage.ChatId = chatId;
        chatMessage.MessageType = MessageType.Message;
        chatMessage.CreatedAt = DateTime.UtcNow;

        var createdChatMessage = await _chatMessageRepository.Create(chatMessage);
        await _chatHub.Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", new
        {
            chatMessage.ChatId,
            chatMessage.UserId,
            chatMessage.MessageType,
            chatMessage.Message,
            chatMessage.CreatedAt
        });
        return createdChatMessage;
    }

    public async Task<ChatMessage> DeleteMessage(int chatId, int id)
    {
        var chat = await _chatRepository.GetById(chatId);
        var chatMessage = await _chatMessageRepository.GetById(id);

        var deletedChatMessage = await _chatMessageRepository.Delete(id);
        return deletedChatMessage;
    }

    public async Task<ChatMessage> EditMessage(int chatId, int id, ChatMessageEditDto chatMessageDto)
    {
        var chat = await _chatRepository.GetById(chatId);
        var chatMessage = await _chatMessageRepository.GetById(id);
        chatMessage.Message = chatMessageDto.Message;

        var updatedMessage = await _chatMessageRepository.Update(chatMessage.Id, chatMessage);
        return updatedMessage;
    }

    public async Task<IEnumerable<ChatMessage>> GetMessages(int chatId)
    {
        var messages = await _chatMessageRepository.GetAll();
        messages = messages.Where(cm => cm.ChatId == chatId).OrderBy(cm => cm.CreatedAt);
        return messages;
    }
}
