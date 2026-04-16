using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return await _context.Products
    .Include(p => p.Supplier)
    .Include(p => p.Category)
    .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products
    .Include(p => p.Supplier)
    .Include(p => p.Category)
    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            if (product.Quantity < 0)
                return BadRequest("Số lượng phải >= 0");

            if (product.ImportPrice < 0 || product.PromotionPrice < 0)
                return BadRequest("Giá không hợp lệ");

            bool exists = await _context.Products
                .AnyAsync(p => p.ProductCode ==  product.ProductCode);

            if (exists)
                return BadRequest("Mã sản phẩm đã tồn tại");
            
            bool nameExists = await _context.Products
                .AnyAsync(p => p.ProductName == product.ProductName);

            if (nameExists)
                return BadRequest("Tên sản phẩm đã tồn tại");

            product.ExpiryDate = DateTime.SpecifyKind(product.ExpiryDate, DateTimeKind.Utc);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest("ID không khớp");

            var existing = await _context.Products.FindAsync(id);

            if (existing == null)
                return NotFound("Không tìm thấy sản phẩm");

            bool codeExists = await _context.Products
                .AnyAsync(p => p.ProductCode == product.ProductCode && p.Id != id);

            if (codeExists)
                return BadRequest("Mã sản phẩm đã tồn tại");

            bool nameExists = await _context.Products
                .AnyAsync(p => p.ProductName == product.ProductName && p.Id != id);

            if (nameExists)
                return BadRequest("Tên sản phẩm đã tồn tại");

            if (product.Quantity < 0)
                return BadRequest("Số lượng phải >= 0");

            if (product.ImportPrice < 0 || product.PromotionPrice < 0)
                return BadRequest("Giá không hợp lệ");

            existing.ProductName = product.ProductName;
            existing.ProductCode = product.ProductCode;
            existing.Quantity = product.Quantity;
            existing.ImportPrice = product.ImportPrice;
            existing.PromotionPrice = product.PromotionPrice;
            existing.ExpiryDate = DateTime.SpecifyKind(product.ExpiryDate, DateTimeKind.Utc);
            existing.SupplierId = product.SupplierId;
            existing.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Xóa thành công");
        }
    }
}
