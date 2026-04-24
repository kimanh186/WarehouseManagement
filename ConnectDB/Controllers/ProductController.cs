using ConnectDB.Data;
using ConnectDB.DTO;
using ConnectDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
        {
            var product = new Product
            {
                ProductCode = dto.ProductCode,
                ProductName = dto.ProductName,
                SupplierId = dto.SupplierId,
                CategoryId = dto.CategoryId,
                Quantity = 0,
                ImportPrice = dto.ImportPrice,
                PromotionPrice = dto.PromotionPrice,
                ExpiryDate = DateTime.SpecifyKind(dto.ExpiryDate, DateTimeKind.Utc)
            };


            if (dto.ImportPrice < 0 || dto.PromotionPrice < 0)
                return BadRequest("Giá không hợp lệ");

            bool codeExists = await _context.Products
                .AnyAsync(p => p.ProductCode == dto.ProductCode);

            if (codeExists)
                return BadRequest("Mã sản phẩm đã tồn tại");

            bool nameExists = await _context.Products
                .AnyAsync(p => p.ProductName == dto.ProductName);

            if (nameExists)
                return BadRequest("Tên sản phẩm đã tồn tại");

            if (dto.Image != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }


        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateDto dto)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return NotFound();

            // map dữ liệu
            existing.ProductName = dto.ProductName;
            existing.ProductCode = dto.ProductCode;
            existing.ImportPrice = dto.ImportPrice;
            existing.PromotionPrice = dto.PromotionPrice;
            existing.SupplierId = dto.SupplierId;
            existing.CategoryId = dto.CategoryId;
            existing.ExpiryDate = DateTime.SpecifyKind(dto.ExpiryDate, DateTimeKind.Utc);


            if (dto.ImportPrice < 0 || dto.PromotionPrice < 0)
                return BadRequest("Giá không hợp lệ");

            bool codeExists = await _context.Products
                .AnyAsync(p => p.ProductCode == dto.ProductCode && p.Id != id);

            if (codeExists)
                return BadRequest("Mã sản phẩm đã tồn tại");

            bool nameExists = await _context.Products
                .AnyAsync(p => p.ProductName == dto.ProductName && p.Id != id);

            if (nameExists)
                return BadRequest("Tên sản phẩm đã tồn tại");

            // xử lý ảnh
            if (dto.Image != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                existing.ImageUrl = "/images/" + fileName;
            }

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
