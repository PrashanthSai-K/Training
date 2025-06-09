using System;
using ClinicManagement.Repository;
using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class AuditLogRepository : Repository<int, AuditLog>
{
    public AuditLogRepository(ChatDbContext chatDbContext) : base(chatDbContext)
    {
    }

    public override async Task<IEnumerable<AuditLog>> GetAll()
    {
        var logs = await _chatDbContext.AuditLogs.ToListAsync()
                ?? throw new EntityEmptyException("No Audit Logs messages found in database");
        return logs;
    }

    public override async Task<AuditLog> GetById(int id)
    {
        var log = await _chatDbContext.AuditLogs.FirstOrDefaultAsync(a => a.Id == id)
                        ?? throw new ItemNotFoundException($"Audit Log with Id : {id} not found");
        return log;
    }
}
