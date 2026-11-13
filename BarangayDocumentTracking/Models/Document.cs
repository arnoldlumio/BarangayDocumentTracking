
using System; 
using System.ComponentModel.DataAnnotations;

namespace BarangayDocumentTracking.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Document code is required.")]
        [Display(Name = "Document Code")]
        public string DocumentCode { get; set; }

        [Required(ErrorMessage = "Document name is required.")]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Display(Name = "Requirements")]
        public string Requirements { get; set; }

        [Display(Name = "Processing Time")]
        public string ProcessingTime { get; set; }

        [Required(ErrorMessage = "Fee is required.")]
        [Range(0, 999999, ErrorMessage = "Fee must be 0 or higher.")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
