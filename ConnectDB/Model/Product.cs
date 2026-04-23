using ConnectDB.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ConnectDB.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string ProductName { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int Quantity { get; set; }

    public decimal ImportPrice { get; set; }

    public decimal PromotionPrice { get; set; }

    public DateTime ExpiryDate { get; set; }
    public string? ImageUrl { get; set; }
    [JsonIgnore]
    public List<ImportOrderDetail>? ImportOrderDetails { get; set; }

    [JsonIgnore]
    public List<ExportOrderDetail>? ExportOrderDetails { get; set; }
}
