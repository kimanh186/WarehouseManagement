namespace ConnectDB.Model
{
    public class ExportOrder
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "completed";
        public bool IsPrinted { get; set; } = false;

        public List<ExportOrderDetail>? Details { get; set; }
    }
}
