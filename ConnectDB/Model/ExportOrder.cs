namespace ConnectDB.Model
{
    public class ExportOrder
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Type { get; set; } = string.Empty;

        public List<ExportOrderDetail>? Details { get; set; }
    }
}
