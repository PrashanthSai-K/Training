using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Interfaces;

public interface IColorsService
{
    Task<IEnumerable<Color>> GetColors();
    Task<Color> GetColorById(int id);
    Task<Color> CreateColor(ColorCreateDto colorDto);
    Task<Color> UpdateColor(int id, ColorUpdateDto colorDto);
    Task<Color> DeleteColor(int id);
    bool ColorExists(int id);
}