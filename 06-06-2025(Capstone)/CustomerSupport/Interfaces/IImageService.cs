using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IImageService
{
    public Task<string> UploadImage(string userId, int chatId, ImageUploadDto imageDto);
    public Task<ImageReponseDto> DownloadImage(string userId, int chatId, string imageName);
    public Task<Image> DeleteImage(string userId, int chatId, string imageName);
}
