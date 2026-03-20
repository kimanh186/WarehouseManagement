using ConnectDB.Models;

namespace ConnectDB.Model
{
    public class ImportOrderDetail
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        public decimal ImportPrice { get; set; }
    }
}
