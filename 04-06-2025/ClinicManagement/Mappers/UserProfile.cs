using System;
using AutoMapper;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<DoctorAddRequestDto, User>()
            .ForMember(d => d.UserName, a => a.MapFrom(src => src.Email))
            .ForMember(d => d.Password, opt => opt.Ignore());

        CreateMap<User, DoctorAddRequestDto>()
            .ForMember(d => d.Email, a => a.MapFrom(src => src.UserName));

        CreateMap<PatientAddRequestDto, User>()
            .ForMember(d => d.UserName, a => a.MapFrom(src => src.Email))
            .ForMember(d => d.Password, opt => opt.Ignore());

        CreateMap<User, PatientAddRequestDto>()
            .ForMember(d => d.Email, a => a.MapFrom(src => src.UserName));

    }
}
