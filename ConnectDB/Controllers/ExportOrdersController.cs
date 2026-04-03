using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Model;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExportOrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExportOrder>>> GetAll()
        {
            return await _context.ExportOrders
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExportOrder>> GetById(int id)
        {
            var order = await _context.ExportOrders
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound("Không tìm thấy phiếu xuất");

            return order;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExportOrder order)
        {
            var validTypes = new[] { "BanHang", "Huy", "ChuyenKho" };

            if (!validTypes.Contains(order.Type))
                return BadRequest("Loại xuất chỉ được: BanHang, Huy, ChuyenKho");

            if (order.Details == null || !order.Details.Any())
                return BadRequest("Phiếu xuất phải có ít nhất 1 sản phẩm");

            foreach (var detail in order.Details)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductCode == detail.ProductCode);

                if (product == null)
                    return BadRequest($"Không tìm thấy sản phẩm có mã {detail.ProductCode}");

                if (detail.Quantity <= 0)
                    return BadRequest("Số lượng xuất phải > 0");

                if (product.Quantity < detail.Quantity)
                    return BadRequest($"Sản phẩm {product.ProductName} không đủ tồn kho");

                if (order.Type == "BanHang" && product.ExpiryDate < DateTime.Now)
                    return BadRequest($"Sản phẩm {product.ProductName} đã hết hạn, không được bán");

                product.Quantity -= detail.Quantity;

                detail.Product = product;
            }

            order.CreatedDate = DateTime.Now;

            _context.ExportOrders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo phiếu xuất thành công",
                data = order
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.ExportOrders
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound("Không tìm thấy phiếu xuất");

            foreach (var detail in order.Details!)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductCode == detail.ProductCode);

                if (product != null)
                {
                    product.Quantity += detail.Quantity;
                }
            }

            _context.ExportOrders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("Xóa phiếu xuất thành công");
        }
    }
}