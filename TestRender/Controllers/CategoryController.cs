using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRender.Data;
using TestRender.Models.DTO;
using TestRender.Models.Entities;

namespace TestRender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await context.Categories
                    .Select(x => new CategoryDTO.CategoryDTO(x.Id, x.Name))
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
                var data = await context.Categories
                    .Select(x => new CategoryDTO.CategoryDTO(x.Id, x.Name))
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTO.AddCategoryDTO addCategoryDTO)
        {
            try
            {
                var category = new Category
                {
                    Name = addCategoryDTO.Name,
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                var result = new CategoryDTO.CategoryDTO(category.Id, category.Name);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTO.UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                var category = await context.Categories.FirstAsync(x => x.Id.Equals(updateCategoryDTO.Id));
                category.Name = updateCategoryDTO.Name;

                await context.SaveChangesAsync();

                var result = new CategoryDTO.CategoryDTO(category.Id, category.Name);

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
                var category = await context.Categories.FirstAsync(x => x.Id.Equals(id));
                context.Categories.Remove(category);

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
