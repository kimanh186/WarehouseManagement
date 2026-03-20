namespace ConnectDB.Model
{
    public class ExportOrder
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string Type { get; set; } = string.Empty; // BanHang, Huy, ChuyenKho

        public List<ExportOrderDetail>? Details { get; set; }
    }
}
