using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRender.Data;
using TestRender.Models.DTO;
using TestRender.Models.Entities;

namespace TestRender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await context.Products
                    .Select(x => new ProductDTO.ProductDataDTO(x.Id, x.Name , x.Description , x.Price , x.CategoryId))
                    .ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await context.Products
                    .Select(x => new ProductDTO.ProductDataDTO(x.Id, x.Name, x.Description, x.Price, x.CategoryId))
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProductDTO.AddProductDTO addProductDTO)
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
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();

                var result = new ProductDTO.ProductDataDTO(product.Id, product.Name, product.Description, product.Price, product.CategoryId))

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(ProductDTO.UpdateProductDTO updateProductDTO)
        {
            try
            {
                var product = await context.Products.FirstAsync(x => x.Id.Equals(updateProductDTO.Id));
                product.Name = updateProductDTO.Name;
                product.Description = updateProductDTO.Description;
                product.Price = updateProductDTO.Price;
                product.CategoryId = updateProductDTO.CategoryId;

                await context.SaveChangesAsync();

                var result = new ProductDTO.ProductDataDTO(product.Id, product.Name, product.Description, product.Price, product.CategoryId))

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await context.Products.FirstAsync(x => x.Id.Equals(id));
                context.Products.Remove(product);

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
