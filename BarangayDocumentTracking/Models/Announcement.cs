using System.ComponentModel.DataAnnotations;

namespace BarangayDocumentTracking.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Announcement title is required.")]
        [Display(Name = "Announcement Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Announcement content is required.")]
        [Display(Name = "Announcement Content")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Date posted is required.")]
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
