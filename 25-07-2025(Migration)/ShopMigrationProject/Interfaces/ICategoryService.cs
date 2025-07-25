using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategories(int page, int pageSize);
    Task<Category> GetCategoryById(int id);
    Task<Category> CreateCategory(CategoryCreateDto categoryDto);
    Task<Category> UpdateCategory(int id, CategoryUpdateDto categoryDto);
    Task<Category> DeleteCategory(int id);
}