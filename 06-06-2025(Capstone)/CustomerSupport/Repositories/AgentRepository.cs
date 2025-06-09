using System;
using ClinicManagement.Repository;
using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class AgentRepository : Repository<int, Agent>
{
    public AgentRepository(ChatDbContext chatDbContext) : base(chatDbContext)
    {
    }

    public override async Task<IEnumerable<Agent>> GetAll()
    {
        var agents = await _chatDbContext.Agents.ToListAsync()
                        ?? throw new EntityEmptyException("No Agents found in database");
        return agents;
    }

    public override async Task<Agent> GetById(int id)
    {
        var agent = await _chatDbContext.Agents.FirstOrDefaultAsync(a => a.Id == id)
                        ?? throw new ItemNotFoundException($"Agent with Id : {id} not found");
        return agent;
    }
}
