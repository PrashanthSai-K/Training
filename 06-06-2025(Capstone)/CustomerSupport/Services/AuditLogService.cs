using System;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;

namespace CustomerSupport.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IRepository<int, AuditLog> _auditLogRepository;

    public AuditLogService(IRepository<int, AuditLog> auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }
    public async Task<AuditLog> CreateAuditLog(AuditLog auditLog)
    {
        var log = await _auditLogRepository.Create(auditLog);
        return log;
    }

    public async Task<AuditLog> DeleteAuditLog(int id)
    {
        var log = await _auditLogRepository.GetById(id);
        await _auditLogRepository.Delete(id);
        return log;
    }

    public async Task<IEnumerable<AuditLog>> GetAuditLogs()
    {
        var logs = await _auditLogRepository.GetAll();
        return logs;
    }
}
