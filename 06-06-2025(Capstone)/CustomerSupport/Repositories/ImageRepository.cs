using System;
using ClinicManagement.Repository;
using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class ImageRepository : Repository<string, Image>
{
    public ImageRepository(ChatDbContext chatDbContext) : base(chatDbContext)
    {
    }

    public override async Task<IEnumerable<Image>> GetAll()
    {
        var images = await _chatDbContext.Images.ToListAsync()
                ?? throw new EntityEmptyException("No Chat messages found in database");
        return images;
    }

    public override async Task<Image> GetById(string imageName)
    {
        var image = await _chatDbContext.Images.FirstOrDefaultAsync(a => a.ImageName == imageName)
                        ?? throw new ItemNotFoundException($"Image with name : {imageName} not found");
        return image;
    }
}
