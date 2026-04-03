using ConnectDB.Models;

namespace ConnectDB.Model
{
    public class ExportOrderDetail
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
