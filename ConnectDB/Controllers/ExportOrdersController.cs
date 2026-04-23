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
            if (string.IsNullOrEmpty(order.Reason))
                return BadRequest("Vui lòng nhập lý do xuất");

            var validReasons = new[] { "sale", "internal", "damaged", "transfer" };
            var reason = order.Reason.ToLower();

            if (!validReasons.Contains(reason))
                return BadRequest("Lý do xuất phải là: sale, internal, damaged, transfer");

            if (order.Details == null || !order.Details.Any())
                return BadRequest("Phiếu xuất phải có ít nhất 1 sản phẩm");

            foreach (var detail in order.Details)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductCode == detail.ProductCode);

                if (product == null)
                    return BadRequest($"Không tìm thấy sản phẩm {detail.ProductCode}");

                if (detail.Quantity <= 0)
                    return BadRequest("Số lượng phải > 0");

                if (product.Quantity < detail.Quantity)
                    return BadRequest($"Sản phẩm {product.ProductName} không đủ tồn kho");

                // 🔥 chỉ check hạn nếu KHÔNG phải hàng hỏng
                if (reason != "damaged" && product.ExpiryDate < DateTime.UtcNow)
                {
                    return BadRequest($"Sản phẩm {product.ProductName} đã hết hạn");
                }

                product.Quantity -= detail.Quantity;

                detail.Product = product;
            }

            order.CreatedDate = DateTime.UtcNow;
            order.Status = "completed";

            _context.ExportOrders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo phiếu xuất thành công",
                data = order
            });
        }

        [HttpPut("{id}/print")]
        public async Task<IActionResult> MarkAsPrinted(int id)
        {
            var order = await _context.ExportOrders.FindAsync(id);

            if (order == null)
                return NotFound("Không tìm thấy phiếu");

            order.IsPrinted = true;

            await _context.SaveChangesAsync();

            return Ok("Đã đánh dấu đã in");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.ExportOrders
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound("Không tìm thấy phiếu xuất");

            //  chặn xóa
            if (order.IsPrinted)
                return BadRequest("Phiếu đã in, không được xóa");

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