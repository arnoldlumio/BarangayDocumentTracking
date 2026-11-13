namespace BarangayDocumentTracking.Models
{
    public class OutgoingDocument
    {
        public int Id { get; set; }

        public string DocumentNumber { get; set; }

        public string Recipient { get; set; }

        public string Subject { get; set; }

        public DateTime DateReleased { get; set; }

        public string Status { get; set; }

        public string PdfFileName { get; set; }
    }
}
