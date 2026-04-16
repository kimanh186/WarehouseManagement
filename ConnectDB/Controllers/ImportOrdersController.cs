using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Model;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImportOrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportOrder>>> GetAll()
        {
            return await _context.ImportOrders
                .Include(x => x.Supplier)
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImportOrder>> GetById(int id)
        {
            var order = await _context.ImportOrders
                .Include(x => x.Supplier)
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound("Không tìm thấy phiếu nhập");

            return order;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImportOrder order)
        {
            var supplier = await _context.Suppliers.FindAsync(order.SupplierId);

            if (supplier == null)
                return BadRequest("Nhà cung cấp không tồn tại");

            if (order.Details == null || !order.Details.Any())
                return BadRequest("Phiếu nhập phải có ít nhất 1 sản phẩm");

            foreach (var detail in order.Details)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductCode == detail.ProductCode);

                if (product == null)
                    return BadRequest($"Không tìm thấy sản phẩm có mã {detail.ProductCode}");

                if (detail.Quantity <= 0)
                    return BadRequest("Số lượng nhập phải > 0");

                if (detail.ImportPrice < 0)
                    return BadRequest("Giá nhập phải >= 0");

                product.Quantity += detail.Quantity;
                product.ImportPrice = detail.ImportPrice;

                detail.Product = product;
            }

            order.CreatedDate = DateTime.UtcNow;

            _context.ImportOrders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo phiếu nhập thành công",
                data = order
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.ImportOrders
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound("Không tìm thấy phiếu nhập");

            foreach (var detail in order.Details!)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductCode == detail.ProductCode);

                if (product != null)
                {
                    if (product.Quantity < detail.Quantity)
                        return BadRequest("Không thể xóa vì tồn kho hiện tại không đủ để rollback");

                    product.Quantity -= detail.Quantity;
                }
            }

            _context.ImportOrders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("Xóa phiếu nhập thành công");
        }
    }
}
