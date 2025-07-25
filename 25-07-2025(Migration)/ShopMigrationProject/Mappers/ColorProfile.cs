using AutoMapper;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Mappers;

public class ColorProfile : Profile
{
    public ColorProfile()
    {
        CreateMap<ColorCreateDto, Color>();
        CreateMap<ColorUpdateDto, Color>();
    }
}