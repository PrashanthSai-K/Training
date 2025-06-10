using System;
using CustomerSupport.Models;

namespace CustomerSupport.Interfaces;

public interface IAuditLogService
{
    public Task<AuditLog> CreateAuditLog(AuditLog auditLog);
    public Task<AuditLog> DeleteAuditLog(int id);
    public Task<IEnumerable<AuditLog>> GetAuditLogs();
}
