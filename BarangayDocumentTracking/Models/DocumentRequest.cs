using System.ComponentModel.DataAnnotations;

namespace BarangayDocumentTracking.Models
{
    public class DocumentRequest
    {
        public int Id { get; set; }

        [Display(Name = "Request Number")]
        public string RequestNumber { get; set; }

        [Required(ErrorMessage = "Please select a document.")]
        [Display(Name = "Document")]
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string ResidentName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Purpose is required.")]
        public string Purpose { get; set; }

        public string Status { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? ProcessedDate { get; set; }

        // Relationship to Documents table
        public Document Document { get; set; }
    }
}
