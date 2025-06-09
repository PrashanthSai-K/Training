using System;
using AutoMapper;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Mappers;

public class ChatMessageMapper : Profile
{
    public ChatMessageMapper()
    {
        CreateMap<ChatMessageCreateDto, ChatMessage>();
    }
}
