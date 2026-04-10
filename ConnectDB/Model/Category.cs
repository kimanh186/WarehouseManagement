using System.Text.Json.Serialization;

namespace ConnectDB.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}