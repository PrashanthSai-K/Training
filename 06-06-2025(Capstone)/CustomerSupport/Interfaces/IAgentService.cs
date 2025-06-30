using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;

namespace CustomerSupport.Interfaces;

public interface IAgentService
{
    public Task<Agent> CreateAgent(AgentRegisterDto agent);
    public Task<Agent> UpdateAgent(string? userId, int id, AgentUpdateDto agent);
    public Task<Agent> ActivateAgent(string? userId, int id);
    public Task<Agent> DeactivateAgent(string? userId, int id);
    public Task<Agent> DeleteAgent(string? userId, int id);
    public Task<IEnumerable<Agent>> GetAgents(AgentQueryParams queryParams);
    public Task<Agent> GetAgentById(int id);
    public Task<Agent> GetAgentFromToken(string userId);
}
