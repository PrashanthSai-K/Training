using System;
using CustomerSupport.Exceptions;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Services;

public class ImageService : IImageService
{
    private readonly IRepository<string, Image> _imageRepository;
    private readonly IRepository<int, Chat> _chatRepository;
    private readonly IRepository<int, ChatMessage> _chatMessageRepository;
    private readonly IRepository<string, User> _userRepository;

    public ImageService(IRepository<string, Image> imageRepository, IRepository<int, Chat> chatRepository, IRepository<int, ChatMessage> chatMessageRepository, IRepository<string, User> userRepository)
    {
        _imageRepository = imageRepository;
        _chatRepository = chatRepository;
        _chatMessageRepository = chatMessageRepository;
        _userRepository = userRepository;
    }

    public async Task<Image> DeleteImage(int chatId, string imageName)
    {
        var chat = await _chatRepository.Delete(chatId);
        var deletedImage = await _imageRepository.Delete(imageName);

        return deletedImage;
    }

    public async Task<ImageReponseDto> DownloadImage(int chatId, string imageName)
    {
        var chat = await _chatRepository.GetById(chatId);

        var image = await _imageRepository.GetById(imageName);
        return new ImageReponseDto()
        {
            ImageName = image.ImageName,
            ImageUrl = $"data:image/jpeg;base64,{Convert.ToBase64String(image.ImageData)}"
        };
    }

    public async Task<string> UploadImage(int chatId, ImageUploadDto imageDto)
    {
        var chat = await _chatRepository.GetById(chatId);
        var user = await _userRepository.GetById(imageDto.UserId);

        var allowedExtentions = new[] { ".jpg", ".jpeg", ".png" };
        string fileName = imageDto.formFile.FileName;
        Console.WriteLine(fileName);

        if (!allowedExtentions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
            throw new UnsupportedFileUploadException("Only images are allowed");

        var memoryStream = new MemoryStream();
        await imageDto.formFile.CopyToAsync(memoryStream);

        var image = new Image();
        image.ImageData = memoryStream.ToArray();
        image.ImageName = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}-{fileName}";
        image.CreatedOn = DateTime.UtcNow;

        var createdImage = await _imageRepository.Create(image);
        await CreateImageMessage(chatId, createdImage.ImageName, imageDto.UserId);
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
        return await _chatMessageRepository.Create(chatMessage);
    }
}
