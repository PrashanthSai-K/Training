using System;
using FileHandling.Context;
using FileHandling.Interfaces;
using FileHandling.Models;
using Microsoft.EntityFrameworkCore;

namespace FileHandling.Repository;

public class FilesRepository : Repository<int, UploadedFile>
{
    public FilesRepository(FileHandlingContext fileHandlingContext) : base(fileHandlingContext)
    {

    }
    public override async Task<IEnumerable<UploadedFile>> GetAll()
    {
        var files = await _context.Files.ToListAsync() ?? throw new Exception("No files found in db");
        return files;
    }

    public override Task<UploadedFile> GetById(int id)
    {
        throw new NotImplementedException();
    }
}
