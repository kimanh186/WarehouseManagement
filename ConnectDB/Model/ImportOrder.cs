namespace ConnectDB.Model
{
    public class ImportOrder
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public List<ImportOrderDetail>? Details { get; set; }

        public bool IsPrinted { get; set; } = false;
    }
}
