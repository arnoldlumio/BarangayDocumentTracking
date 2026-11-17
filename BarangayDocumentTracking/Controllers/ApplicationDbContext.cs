using BarangayDocumentTracking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarangayDocumentTracking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentRequest> DocumentRequests { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

    }
}