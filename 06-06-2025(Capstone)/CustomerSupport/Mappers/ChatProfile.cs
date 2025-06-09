using System;
using AutoMapper;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Mappers;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<ChatCreationDto, Chat>();
    }
}
