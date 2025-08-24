using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading;
using TestRender.Data;
using TestRender.Models.DTO;
using TestRender.Models.Entities;

namespace TestRender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(
            MainDbContext context,
            IDistributedCache cache
        ) : ControllerBase
    {
        private const string REDIS_KEY = "Categories";

        [HttpGet("cache")]
        public async Task<IActionResult> GetFromCache(CancellationToken cancellationToken)
        {
            try
            {
                var data = await cache.GetStringAsync(REDIS_KEY, cancellationToken);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var data = await context.Products
                    .Select(x => new ProductDTO.ProductDataDTO(x.Id, x.Name , x.Description , x.Price , x.CategoryId))
                    .ToListAsync(cancellationToken);

                await cache.SetStringAsync(REDIS_KEY, JsonSerializer.Serialize(data), cancellationToken);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var data = await context.Products
                    .Select(x => new ProductDTO.ProductDataDTO(x.Id, x.Name, x.Description, x.Price, x.CategoryId))
                    .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProductDTO.AddProductDTO addProductDTO, CancellationToken cancellationToken)
        {
            try
            {
                var product = new Product
                {
                    Name = addProductDTO.Name,
                    Description = addProductDTO.Description,
                    Price = addProductDTO.Price,
                    CategoryId = addProductDTO.CategoryId
                };
                await context.Products.AddAsync(product, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                var result = new ProductDTO.ProductDataDTO(product.Id, product.Name, product.Description, product.Price, product.CategoryId))

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(ProductDTO.UpdateProductDTO updateProductDTO, CancellationToken cancellationToken)
        {
            try
            {
                var product = await context.Products.FirstAsync(x => x.Id.Equals(updateProductDTO.Id), cancellationToken);
                product.Name = updateProductDTO.Name;
                product.Description = updateProductDTO.Description;
                product.Price = updateProductDTO.Price;
                product.CategoryId = updateProductDTO.CategoryId;

                await context.SaveChangesAsync(cancellationToken);

                var result = new ProductDTO.ProductDataDTO(product.Id, product.Name, product.Description, product.Price, product.CategoryId))

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await context.Products.FirstAsync(x => x.Id.Equals(id));
                context.Products.Remove(product);

                await context.SaveChangesAsync(cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
