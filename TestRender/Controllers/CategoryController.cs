using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TestRender.Data;
using TestRender.Models.DTO;
using TestRender.Models.Entities;

namespace TestRender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(
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
                var data = await context.Categories
                    .Select(x => new CategoryDTO.CategoryDataDTO(x.Id, x.Name))
                    .ToListAsync();

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
                var data = await context.Categories
                    .Select(x => new CategoryDTO.CategoryDataDTO(x.Id, x.Name))
                    .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTO.AddCategoryDTO addCategoryDTO, CancellationToken cancellationToken)
        {
            try
            {
                var category = new Category
                {
                    Name = addCategoryDTO.Name,
                };
                await context.Categories.AddAsync(category, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                var result = new CategoryDTO.CategoryDataDTO(category.Id, category.Name);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTO.UpdateCategoryDTO updateCategoryDTO, CancellationToken cancellationToken)
        {
            try
            {
                var category = await context.Categories.FirstAsync(x => x.Id.Equals(updateCategoryDTO.Id), cancellationToken);
                category.Name = updateCategoryDTO.Name;

                await context.SaveChangesAsync(cancellationToken);

                var result = new CategoryDTO.CategoryDataDTO(category.Id, category.Name);

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
                var category = await context.Categories.FirstAsync(x => x.Id.Equals(id));
                context.Categories.Remove(category);

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
