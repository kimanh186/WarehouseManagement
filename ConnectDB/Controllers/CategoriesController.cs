using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            bool exists = await _context.Categories
                .AnyAsync(x => x.CategoryName == category.CategoryName);

            if (exists)
                return BadRequest("Danh mục đã tồn tại");

            _context.Categories.Add(category);
            await _context.SaveChangesAsynsc();

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            var existing = await _context.Categories.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.CategoryName = category.CategoryName;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Xóa danh mục thành công");
        }
    }
}