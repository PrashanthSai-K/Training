using System;
using Azure.Storage.Blobs;
using CustomerSupport.Exceptions;
using CustomerSupport.Interfaces;
using CustomerSupport.MessageHub;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.SignalR;

namespace CustomerSupport.Services;

public class ImageService : IImageService
{
    private readonly IRepository<string, Image> _imageRepository;
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<int, ChatMessage> _chatMessageRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly IOtherContextFunctions _otherContextFUnctions;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly IRepository<string, User> _userRepository;
    private readonly BlobContainerClient _containerClient;

    public ImageService(IRepository<string, Image> imageRepository,
                        IRepository<int, Chat> chatRepository,
                        IRepository<int, ChatMessage> chatMessageRepository,
                        IAuditLogService auditLogService,
                        IOtherContextFunctions otherContextFunctions,
                        IHubContext<ChatHub> chatHub,
                        IRepository<string, User> userRepository,
                        IConfiguration configuration)
    {
        _imageRepository = imageRepository;
        _chatRepository = chatRepository;
        _chatMessageRepository = chatMessageRepository;
        _auditLogService = auditLogService;
        _otherContextFUnctions = otherContextFunctions;
        _chatHub = chatHub;
        _userRepository = userRepository;
        var sasUrl = configuration["Azure:ContainerUrl"] ?? throw new ItemNotFoundException("Azure Sas url not found");
        _containerClient = new BlobContainerClient(new Uri(sasUrl));
    }

    public async Task<Image> DeleteImage(string userId, int chatId, string imageName)
    {
        var chat = await _chatRepository.Delete(chatId);

        var isUserInChat = await _otherContextFUnctions.IsUserInChat(chatId, userId);
        if (!isUserInChat)
            throw new UnauthorizedAccessException("User not authorized to upload image to this chat");

        var deletedImage = await _imageRepository.Delete(imageName);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Delete", Entity = "Image", CreatedOn = DateTime.UtcNow });

        return deletedImage;
    }

    public async Task<ImageReponseDto> DownloadImage(string userId, int chatId, string imageName)
    {
        var chat = await _chatRepository.GetById(chatId);

        var isUserInChat = await _otherContextFUnctions.IsUserInChat(chatId, userId);
        if (!isUserInChat)
            throw new UnauthorizedAccessException("User not authorized to upload image to this chat");

        var image = await _imageRepository.GetById(imageName);

        return new ImageReponseDto()
        {
            ImageName = image.ImageName,
            ImageUrl = $"data:image/jpeg;base64,{Convert.ToBase64String(image.ImageData)}"
        };
    }

    public async Task<string> UploadImage(string userId, int chatId, ImageUploadDto imageDto)
    {
        var chat = await _chatRepository.GetById(chatId);
        var user = await _userRepository.GetById(userId);

        var isUserInChat = await _otherContextFUnctions.IsUserInChat(chatId, userId);
        if (!isUserInChat)
            throw new UnauthorizedAccessException("User not authorized to upload image to this chat");

        var allowedExtentions = new[] { ".jpg", ".jpeg", ".png" };
        string fileName = imageDto.formFile.FileName;

        if (!allowedExtentions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
            throw new UnsupportedFileUploadException("Only images are allowed");

        var memoryStream = new MemoryStream();
        await imageDto.formFile.CopyToAsync(memoryStream);

        var image = new Image();
        image.ImageData = memoryStream.ToArray();
        image.ImageName = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}-{fileName}";
        image.CreatedOn = DateTime.UtcNow;

        var createdImage = await _imageRepository.Create(image);
        await CreateImageMessage(chatId, createdImage.ImageName, userId);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = user.Username, Action = "Create", Entity = "Image", CreatedOn = DateTime.UtcNow });

        return "Image Uploaded";
    }


    public async Task<ChatMessage> CreateImageMessage(int chatId, string imageName, string UserId)
    {
        var chatMessage = new ChatMessage()
        {
            ChatId = chatId,
            ImageName = imageName,
            UserId = UserId,
            MessageType = MessageType.Image,
            CreatedAt = DateTime.UtcNow
        };
        await _chatHub.Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", new
        {
            chatMessage.ChatId,
            chatMessage.UserId,
            chatMessage.MessageType,
            chatMessage.ImageName,
            chatMessage.CreatedAt
        });

        return await _chatMessageRepository.Create(chatMessage);
    }
}
