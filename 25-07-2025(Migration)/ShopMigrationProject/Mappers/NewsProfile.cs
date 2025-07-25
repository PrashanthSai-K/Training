using AutoMapper;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Mappers;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<NewsCreateDto, News>();
        CreateMap<NewsUpdateDto, News>();
    }
}