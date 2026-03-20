using ConnectDB.Model;
using ConnectDB.Models;
using Microsoft.EntityFrameworkCore;

namespace ConnectDB.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<ImportOrder> ImportOrders { get; set; }
    public DbSet<ImportOrderDetail> ImportOrderDetails { get; set; }
    public DbSet<ExportOrder> ExportOrders { get; set; }
    public DbSet<ExportOrderDetail> ExportOrderDetails { get; set; }
}