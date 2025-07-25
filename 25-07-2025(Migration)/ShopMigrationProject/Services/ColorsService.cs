using AutoMapper;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.Services;

public class ColorService : IColorsService
{
    private readonly ChienVHShopDBEntities _context;
    private readonly IMapper _mapper;

    public ColorService(ChienVHShopDBEntities context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public bool ColorExists(int id)
    {
        return _context.Colors.Any(c => c.ColorId == id);
    }

    public async Task<Color> CreateColor(ColorCreateDto colorDto)
    {
        var color = _mapper.Map<Color>(colorDto);
        _context.Colors.Add(color);
        await _context.SaveChangesAsync();
        return color;
    }

    public async Task<Color> DeleteColor(int id)
    {
        var color = await _context.Colors.FindAsync(id);
        if (color == null)
            throw new Exception("Category not found");

        _context.Colors.Remove(color);
        await _context.SaveChangesAsync();

        return color;
    }

    public async Task<Color> GetColorById(int id)
    {
        var color = await _context.Colors.FindAsync(id);

        if (color == null)
            throw new Exception("Color not found");

        return color;
    }

    public async Task<IEnumerable<Color>> GetColors()
    {
        var colors = await _context.Colors
                                  .OrderBy(c => c.Color1)
                                  .ToListAsync();
        return colors;
    }

    public async Task<Color> UpdateColor(int id, ColorUpdateDto colorDto)
    {
        var color = _mapper.Map<Color>(colorDto);
        _context.Entry(color).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Colors.Any(e => e.ColorId == id))
                throw new Exception("Color not found");
            else
                throw new Exception("Something went wrong");
        }

        return color;
    }
}