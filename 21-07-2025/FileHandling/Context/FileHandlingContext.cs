using System;
using FileHandling.Models;
using Microsoft.EntityFrameworkCore;

namespace FileHandling.Context;

public class FileHandlingContext : DbContext
{
    public FileHandlingContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<UploadedFile> Files {get; set;}
}
