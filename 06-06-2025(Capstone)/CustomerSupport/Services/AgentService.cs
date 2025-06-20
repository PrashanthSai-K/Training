using System;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CustomerSupport.Exceptions;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;

namespace CustomerSupport.Services;

public class AgentService : IAgentService
{
    private readonly IRepository<int, Agent> _agentRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IOtherContextFunctions _otherContextFunctionalities;
    private readonly IAuditLogService _auditLogService;
    private readonly IMapper _mapper;

    public AgentService(IRepository<int, Agent> agentRepository,
                        IRepository<string, User> userRepository,
                        IMapper mapper,
                        IAuditLogService auditLogService,
                        IOtherContextFunctions otherContextFunctions,
                        IHashingService hashingService)
    {
        _agentRepository = agentRepository;
        _userRepository = userRepository;
        _hashingService = hashingService;
        _otherContextFunctionalities = otherContextFunctions;
        _auditLogService = auditLogService;
        _mapper = mapper;
    }
    public async Task<Agent> CreateAgent(AgentRegisterDto agentDto)
    {
        if (await _otherContextFunctionalities.IsUsernameExists(agentDto.Email))
            throw new DuplicateEntryException("Email already exists");

        var user = _mapper.Map<AgentRegisterDto, User>(agentDto);
        user.Password = _hashingService.HashData(agentDto.Password);
        user.Roles = "Agent";
        user.Status = "Active";
        
        var agent = _mapper.Map<AgentRegisterDto, Agent>(agentDto);
        agent.Status = "Active";

        var createdUser = await _userRepository.Create(user);
        var createagent = await _agentRepository.Create(agent);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = user.Username, Action = "Create", Entity = "Agent", CreatedOn = DateTime.UtcNow });
        return createagent;
    }

    public async Task<Agent> DeleteAgent(string? userId, int id)
    {
        var existingAgent = await _agentRepository.GetById(id);

        if (existingAgent.Email != userId)
            throw new UnauthorizedAccessException("User not authorized to delete this account details");

        existingAgent.Status = "Deleted";
        var deleteagent = await _agentRepository.Update(id, existingAgent);

        var existingUser = await _userRepository.GetById(deleteagent.Email);
        existingUser.Status = "Deleted";
        await _userRepository.Update(existingAgent.Email, existingUser);

        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Delete", Entity = "Agent", CreatedOn = DateTime.UtcNow });
        return deleteagent;
    }

    public async Task<Agent> GetAgentById(int id)
    {
        var agent = await _agentRepository.GetById(id);
        return agent;
    }

    public async Task<IEnumerable<Agent>> GetAgents(AgentQueryParams queryParams)
    {
        var agents = await _agentRepository.GetAll();

        agents = GetAgentsByName(agents, queryParams.Name);
        agents = GetAgentsByEmail(agents, queryParams.Email);

        agents = agents.Skip((queryParams.PageNumber - 1) * queryParams.PageSize).Take(queryParams.PageSize);

        return agents;
    }

    public async Task<Agent> UpdateAgent(string? userId, int id, AgentUpdateDto agentDto)
    {
        var existingAgent = await _agentRepository.GetById(id);

        if (existingAgent.Email != userId)
            throw new UnauthorizedAccessException("User not authorized to update this account details");

        var agent = _mapper.Map<AgentUpdateDto, Agent>(agentDto);
        agent.Id = existingAgent.Id;
        agent.Email = existingAgent.Email;

        var updatedagent = await _agentRepository.Update(id, agent);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Update", Entity = "Agent", CreatedOn = DateTime.UtcNow });

        return updatedagent;
    }

    public IEnumerable<Agent> GetAgentsByName(IEnumerable<Agent> agents, string? name)
    {
        if (name == null || name.Length == 0)
            return agents;

        return agents.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public IEnumerable<Agent> GetAgentsByEmail(IEnumerable<Agent> agents, string? email)
    {
        if (email == null || email.Length == 0)
            return agents;

        return agents.Where(a => a.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
    }

}
