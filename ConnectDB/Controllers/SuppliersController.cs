using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Model;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAll()
        {
            return await _context.Suppliers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetById(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound("Không tìm thấy nhà cung cấp");

            return supplier;
        }

        [HttpPost]
        public async Task<ActionResult<Supplier>> Create(Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                return BadRequest("Tên nhà cung cấp không được để trống");

            bool exists = await _context.Suppliers
                .AnyAsync(x => x.SupplierName == supplier.SupplierName);

            if (exists)
                return BadRequest("Nhà cung cấp đã tồn tại");

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest("ID không khớp");

            var existing = await _context.Suppliers.FindAsync(id);

            if (existing == null)
                return NotFound("Không tìm thấy nhà cung cấp");

            existing.SupplierName = supplier.SupplierName;
            existing.Phone = supplier.Phone;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound("Không tìm thấy nhà cung cấp");

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return Ok("Xóa nhà cung cấp thành công");
        }
    }
}
