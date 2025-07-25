using AutoMapper;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productsService.GetAllProducts();
            return Ok(products);
        }

        // GET: api/products/by-category/5?page=1&pageSize=10
        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId, int page = 1, int pageSize = 10)
        {
            var products = await _productsService.GetProductsByCategory(categoryId, page, pageSize);
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productsService.GetProductById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        //POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductCreateDto productDto)
        {
            var product = await _productsService.PostProduct(productDto);

            return CreatedAtAction(nameof(GetAllProducts), new { id = product.ProductId }, product);
        }

        //Put: api/products
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductUpdateDto productDto)
        {
            if (id != productDto.ProductId)
                return BadRequest();

            var product = await _productsService.UpdateProduct(id, productDto);

            return Ok(product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productsService.DeleteProduct(id);

            return Ok(product);
        }
    }
}
