using System;
using AutoMapper;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AgentRegisterDto, User>()
            .ForMember(d => d.Username, a => a.MapFrom(s => s.Email))
            .ForMember(d => d.Password, opt => opt.Ignore());

        CreateMap<User, AgentRegisterDto>()
            .ForMember(d => d.Email, a => a.MapFrom(s => s.Username));

        CreateMap<CustomerRegisterDto, User>()
            .ForMember(d => d.Username, a => a.MapFrom(s => s.Email))
            .ForMember(d => d.Password, opt => opt.Ignore());

        CreateMap<User, CustomerRegisterDto>()
            .ForMember(d => d.Email, a => a.MapFrom(s => s.Username));
    }
}
