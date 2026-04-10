using ConnectDB.Model;
using ConnectDB.Models;
using Microsoft.EntityFrameworkCore;

namespace ConnectDB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<ImportOrder> ImportOrders { get; set; }
        public DbSet<ImportOrderDetail> ImportOrderDetails { get; set; }

        public DbSet<ExportOrder> ExportOrders { get; set; }
        public DbSet<ExportOrderDetail> ExportOrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductCode)
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(s => s.Phone)
                .IsUnique();

            modelBuilder.Entity<ImportOrder>()
                .HasMany(i => i.Details)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExportOrder>()
                .HasMany(e => e.Details)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
    .HasOne(p => p.Supplier)
    .WithMany(s => s.Products)
    .HasForeignKey(p => p.SupplierId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
    .HasAlternateKey(p => p.ProductCode);

            modelBuilder.Entity<ImportOrderDetail>()
                .HasOne(d => d.Product)
                .WithMany(p => p.ImportOrderDetails)
                .HasForeignKey(d => d.ProductCode)
                .HasPrincipalKey(p => p.ProductCode);

            modelBuilder.Entity<ExportOrderDetail>()
    .HasOne(d => d.Product)
    .WithMany(p => p.ExportOrderDetails)
    .HasForeignKey(d => d.ProductCode)
    .HasPrincipalKey(p => p.ProductCode);
        }



    }
}
