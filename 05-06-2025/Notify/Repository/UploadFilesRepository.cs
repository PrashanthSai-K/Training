using System;
using Microsoft.EntityFrameworkCore;
using Notify.Context;
using Notify.Models;

namespace Notify.Repository;

public class UploadFilesRepository : Repository<int, Upload>
{
    public UploadFilesRepository(NotifyDbContext dbContext) : base(dbContext)
    {

    }
    public override async Task<IEnumerable<Upload>> GetAll()
    {
        var files = await _context.Uploads.ToListAsync() ?? throw new Exception("No files found in db");
        return files;
    }

    public override Task<Upload> GetById(int id)
    {
        throw new NotImplementedException();
    }
}
