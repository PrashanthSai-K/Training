using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Interfaces;

public interface IProductsService
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetProductsByCategory(int categoryId, int page = 1, int pageSize = 10);
    Task<Product> GetProductById(int id);
    Task<Product> PostProduct(ProductCreateDto productDto);
    Task<Product> UpdateProduct(int id, ProductUpdateDto productDto);
    Task<Product> DeleteProduct(int id);
}