using System.Text.Json.Serialization;
using ConnectDB.Models;

namespace ConnectDB.Model
{
    public class Supplier
    {
        public int Id { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}
