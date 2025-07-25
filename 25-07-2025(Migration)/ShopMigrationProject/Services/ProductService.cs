using AutoMapper;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.Services;

public class ProductService : IProductsService
{
    private readonly ChienVHShopDBEntities _context;
    private readonly IMapper _mapper;

    public ProductService(ChienVHShopDBEntities context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Product> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new Exception("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context.Products
                             .OrderByDescending(p => p.ProductId)
                             .ToListAsync();
    }

    public async Task<Product> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new Exception("Product not found");

        return product;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId, int page = 1, int pageSize = 10)
    {
        var products = await _context.Products
                                     .Where(p => p.CategoryId == categoryId)
                                     .OrderByDescending(p => p.ProductId)
                                     .Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

        return products;
    }

    public async Task<Product> PostProduct(ProductCreateDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateProduct(int id, ProductUpdateDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);

        _context.Entry(product).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categories.Any(e => e.CategoryId == id))
                throw new Exception("Product not found");
            else
                throw;
        }

        return product;
    }
}