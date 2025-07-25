using AutoMapper;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Mappers;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<ModelCreateDto, Model>();
        CreateMap<ModelUpdateDto, Model>();
    }
}