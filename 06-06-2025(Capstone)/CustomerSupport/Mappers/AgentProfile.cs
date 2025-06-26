using System;
using AutoMapper;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Mappers;

public class AgentProfile : Profile
{
    public AgentProfile()
    {
        CreateMap<AgentRegisterDto, Agent>();
        CreateMap<AgentUpdateDto, Agent>();
        CreateMap<Agent, AgentResponseDto>();
    }
}
