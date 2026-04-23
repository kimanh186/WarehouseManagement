using Microsoft.AspNetCore.Http;

namespace ConnectDB.DTO
{
    public class ProductUpdateDto
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal ImportPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public DateTime ExpiryDate { get; set; }

        public IFormFile? Image { get; set; }
    }
}