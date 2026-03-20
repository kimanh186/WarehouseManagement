using System.ComponentModel.DataAnnotations;

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

    public int Quantity { get; set; }

    public decimal ImportPrice { get; set; }

    public decimal PromotionPrice { get; set; }

    public DateTime ExpiryDate { get; set; }
}