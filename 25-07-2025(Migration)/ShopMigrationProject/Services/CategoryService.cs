using AutoMapper;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.Services;

public class CategoryService : ICategoryService
{
    private readonly ChienVHShopDBEntities _context;
    private readonly IMapper _mapper;

    public CategoryService(ChienVHShopDBEntities context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Category> CreateCategory(CategoryCreateDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            throw new Exception("Category not found");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<IEnumerable<Category>> GetCategories(int page, int pageSize)
    {
        var categories = await _context.Categories
                                  .OrderBy(c => c.Name)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        return categories;
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        return category;
    }

    public async Task<Category> UpdateCategory(int id, CategoryUpdateDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categories.Any(e => e.CategoryId == id))
                throw new Exception("Category not found");
            else
                throw new Exception("Something went wrong");
        }

        return category;
    }
}