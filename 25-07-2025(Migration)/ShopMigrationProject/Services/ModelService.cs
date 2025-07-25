using AutoMapper;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.Services;

public class ModelService : IModelService
{
    private readonly ChienVHShopDBEntities _context;
    private readonly IMapper _mapper;

    public ModelService(ChienVHShopDBEntities context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Model> CreateModel(ModelCreateDto modelDto)
    {
        var model = _mapper.Map<Model>(modelDto);
        _context.Models.Add(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<Model> DeleteModel(int id)
    {
        var model = await _context.Models.FindAsync(id);
        if (model == null)
            throw new Exception("Model not found");

        _context.Models.Remove(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task<Model> GetModel(int id)
    {
        var model = await _context.Models.FindAsync(id);

        if (model == null)
            throw new Exception("Model not found");

        return model;
    }

    public async Task<IEnumerable<Model>> GetModels(int page, int pageSize)
    {
        var models = await _context.Models
                                       .OrderBy(c => c.Model1)
                                       .Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
        return models;
    }

    public async Task<Model> UpdateModel(int id, ModelUpdateDto modelDto)
    {
        var model = _mapper.Map<Model>(modelDto);
        _context.Entry(model).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Models.Any(e => e.ModelId == id))
                throw new Exception("Model not found");
            else
                throw;
        }

        return model;
    }
}