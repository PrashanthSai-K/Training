using System;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.MessageHub;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.SignalR;

namespace CustomerSupport.Services;

public class ChatMessageService : IChatMessageService
{
    private readonly IRepository<int, ChatMessage> _chatMessageRepository;
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IOtherContextFunctions _otherContextFunctions;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly IMapper _mapper;

    public ChatMessageService(IRepository<int, ChatMessage> chatMessageRepository,
                              IRepository<int, Chat> chatRepository,
                              IRepository<string, User> userRepository,
                              IOtherContextFunctions otherContextFunctions,
                              IHubContext<ChatHub> chatHub,
                              IMapper mapper)
    {
        _chatMessageRepository = chatMessageRepository;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _otherContextFunctions = otherContextFunctions;
        _chatHub = chatHub;
        _mapper = mapper;
    }
    public async Task<ChatMessage> CreateTextMessage(string? userId, int chatId, ChatMessageCreateDto chatMessageDto)
    {
        var chat = await _chatRepository.GetById(chatId);
        var user = await _userRepository.GetById(userId ?? "");

        var isUserInChat = await _otherContextFunctions.IsUserInChat(chatId, userId ?? "");
        if (!isUserInChat)
            throw new UnauthorizedAccessException("User not authorized to send message to this chat");

        var chatMessage = _mapper.Map<ChatMessageCreateDto, ChatMessage>(chatMessageDto);
        chatMessage.ChatId = chatId;
        chatMessage.UserId = userId ?? "";
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

    public async Task<ChatMessage> DeleteMessage(string? userId, int chatId, int id)
    {
        var chat = await _chatRepository.GetById(chatId);
        var chatMessage = await _chatMessageRepository.GetById(id);

        if (chatMessage.UserId != userId)
            throw new UnauthorizedAccessException("User not authorized to delete message of this chat");

        var deletedChatMessage = await _chatMessageRepository.Delete(id);
        return deletedChatMessage;
    }

    public async Task<ChatMessage> EditMessage(string? userId, int chatId, int id, ChatMessageEditDto chatMessageDto)
    {
        var chat = await _chatRepository.GetById(chatId);
        var chatMessage = await _chatMessageRepository.GetById(id);

        if (chatMessage.UserId != userId)
            throw new UnauthorizedAccessException("User not authorized to edit message of this chat");

        chatMessage.Message = chatMessageDto.Message;

        var updatedMessage = await _chatMessageRepository.Update(chatMessage.Id, chatMessage);
        return updatedMessage;
    }

    public async Task<IEnumerable<ChatMessage>> GetMessages(int chatId, ChatMessageQueryParams queryParams)
    {
        var messages = await _chatMessageRepository.GetAll();
        messages = messages.Where(cm => cm.ChatId == chatId).OrderBy(cm => cm.CreatedAt);

        messages = GetMessagesByUser(messages, queryParams.UserId);
        messages = GetMessagesByMessage(messages, queryParams.Message);
        messages = GetMessagesByDate(messages, queryParams.Date);

        messages = messages.Skip((queryParams.PageNumber - 1) * queryParams.PageSize).Take(queryParams.PageSize);

        return messages;
    }

    public IEnumerable<ChatMessage> GetMessagesByUser(IEnumerable<ChatMessage> messages, string? userId)
    {
        if (userId == null || userId.Length == 0 || messages.Count() == 0)
            return messages;

        messages = messages.Where(message => message.UserId.ToLower() == userId.ToLower());
        return messages;
    }

    public IEnumerable<ChatMessage> GetMessagesByMessage(IEnumerable<ChatMessage> messages, string? message)
    {
        if (message == null || message.Length == 0 || messages.Count() == 0)
            return messages;

        messages = messages.Where(m => !string.IsNullOrEmpty(m.Message) && m.Message.Contains(message, StringComparison.OrdinalIgnoreCase));
        return messages;
    }

    public IEnumerable<ChatMessage> GetMessagesByDate(IEnumerable<ChatMessage> messages, DateRange? date)
    {
        if (date == null || messages.Count() == 0)
            return messages;

        if (date?.From != null && date?.To != null && date.From > date.To)
            return messages;

        if (date?.From != null)
            messages = messages.Where(message => message.CreatedAt.Date >= date.From.Value.Date);

        if (date?.To != null)
            messages = messages.Where(message => message.CreatedAt.Date <= date.To.Value.Date);

        return messages;
    }

}
