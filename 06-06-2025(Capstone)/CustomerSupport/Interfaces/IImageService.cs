using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface IImageService
{
    public Task<string> UploadImage(int chatId, ImageUploadDto imageDto);
    public Task<ImageReponseDto> DownloadImage(int chatId, string imageName);
    public Task<Image> DeleteImage(int chatId, string imageName);
}
