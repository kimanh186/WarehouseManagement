using ConnectDB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("import-by-date")]
        public async Task<IActionResult> ImportByDate(DateTime date)
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            var nextDate = date.AddDays(1);

            var result = await _context.ImportOrders
                .Include(x => x.Supplier)
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .Where(x => x.CreatedDate >= date && x.CreatedDate < nextDate)
                .Select(x => new
                {
                    x.Id,
                    x.CreatedDate,
                    Supplier = x.Supplier!.SupplierName,
                    TotalQuantity = x.Details!.Sum(d => d.Quantity),
                    TotalMoney = x.Details.Sum(d => d.Quantity * d.ImportPrice),
                    Products = x.Details.Select(d => new
                    {
                        d.ProductCode,
                        ProductName = d.Product!.ProductName,
                        d.Quantity,
                        d.ImportPrice
                    })
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("import-by-month")]
        public async Task<IActionResult> ImportByMonth(int month, int year)
        {
            var result = await _context.ImportOrders
                .Include(x => x.Supplier)
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year)
                .Select(x => new
                {
                    x.Id,
                    x.CreatedDate,
                    Supplier = x.Supplier!.SupplierName,
                    TotalQuantity = x.Details!.Sum(d => d.Quantity),
                    TotalMoney = x.Details.Sum(d => d.Quantity * d.ImportPrice)
                })
                .ToListAsync();

            var totalMonthMoney = result.Sum(x => x.TotalMoney);
            var totalMonthQuantity = result.Sum(x => x.TotalQuantity);

            return Ok(new
            {
                Month = month,
                Year = year,
                TotalImportOrders = result.Count,
                TotalQuantity = totalMonthQuantity,
                TotalMoney = totalMonthMoney,
                Data = result
            });
        }

        [HttpGet("expiring-products")]
        public async Task<IActionResult> ExpiringProducts()
        {
            var today = DateTime.UtcNow;
            var next30Days = today.AddDays(30);

            var products = await _context.Products
                .Include(p => p.Supplier)
                //.Where(p => p.ExpiryDate >= today && p.ExpiryDate <= next30Days)
                .Where(p => p.ExpiryDate <= next30Days)

                .Select(p => new
                {
                    p.Id,
                    p.ProductCode,
                    p.ProductName,
                    p.Quantity,
                    p.ExpiryDate,
                    Supplier = p.Supplier!.SupplierName,
                    DaysLeft = (p.ExpiryDate - today).Days,
                    Status = p.ExpiryDate < today ? "Đã hết hạn" : "Sắp hết hạn"
                })
                .OrderBy(p => p.DaysLeft)
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("out-of-stock")]
        public async Task<IActionResult> OutOfStock()
        {
            var products = await _context.Products
                .Include(p => p.Supplier)
                .Where(p => p.Quantity <= 5)
                .Select(p => new
                {
                    p.Id,
                    p.ProductCode,
                    p.ProductName,
                    p.Quantity,
                    Supplier = p.Supplier!.SupplierName,
                    Status = p.Quantity == 0 ? "Hết hàng" : "Sắp hết hàng"
                })
                .OrderBy(p => p.Quantity)
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("export-by-date")]
        public async Task<IActionResult> ExportByDate(DateTime date)
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var nextDate = date.AddDays(1);

            var result = await _context.ExportOrders
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .Where(x => x.CreatedDate >= date && x.CreatedDate < nextDate)
                .Select(x => new
                {
                    x.Id,
                    x.CreatedDate,
                    Reason = x.Reason,
                    TotalQuantity = x.Details!.Sum(d => d.Quantity),
                    Products = x.Details.Select(d => new
                    {
                        d.ProductCode,
                        ProductName = d.Product!.ProductName,
                        d.Quantity
                    })
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("export-by-month")]
        public async Task<IActionResult> ExportByMonth(int month, int year)
        {
            var result = await _context.ExportOrders
                .Include(x => x.Details!)
                .ThenInclude(d => d.Product)
                .Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year)
                .Select(x => new
                {
                    x.Id,
                    x.CreatedDate,
                    Reason = x.Reason,
                    TotalQuantity = x.Details!.Sum(d => d.Quantity)
                })
                .ToListAsync();

            var totalQuantity = result.Sum(x => x.TotalQuantity);

            return Ok(new
            {
                Month = month,
                Year = year,
                TotalExportOrders = result.Count,
                TotalQuantity = totalQuantity,
                Data = result
            });
        }
    }
}