using System;
using AutoMapper;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Mappers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerRegisterDto, Customer>();
        CreateMap<CustomerUpdateDto, Customer>();
    }
}
