using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarangayDocumentTracking.Controllers
{
    public class TrackUserController : Controller
    {
        [HttpGet]
        public IActionResult Index(string trackingNumber)
        {
            // STATIC DOCUMENT DATA
            var documents = new List<DocumentTracking>
            {
                new DocumentTracking
                {
                    Id = 1,
                    TrackingNumber = "DOC-001",
                    DocumentTitle = "Barangay Resolution",
                    DocumentType = "Resolution",
                    DateReceived = "September 15, 2026",
                    Status = "Processing",
                    Priority = "Normal",
                    Source = "Barangay Office",
                    Sender = "Barangay Secretary",
                    CurrentLocation = "Barangay Administration Office",
                    CurrentHandler = "Barangay Secretary",
                    Description = "Barangay Resolution submitted for review and processing.",

                    History = new List<DocumentHistory>
                    {
                        new DocumentHistory
                        {
                            Title = "Document Registered",
                            Description = "The document was successfully registered in the system.",
                            Date = "September 15, 2026",
                            Time = "8:30 AM",
                            Icon = "bi-file-earmark-plus"
                        },

                        new DocumentHistory
                        {
                            Title = "Document Received",
                            Description = "The document was received by the barangay office.",
                            Date = "September 15, 2026",
                            Time = "9:15 AM",
                            Icon = "bi-inbox"
                        },

                        new DocumentHistory
                        {
                            Title = "Processing Started",
                            Description = "The document is currently being processed.",
                            Date = "September 15, 2026",
                            Time = "10:00 AM",
                            Icon = "bi-hourglass-split"
                        },

                        new DocumentHistory
                        {
                            Title = "Assigned to Barangay Secretary",
                            Description = "The document was assigned to the Barangay Secretary for review.",
                            Date = "September 15, 2026",
                            Time = "10:30 AM",
                            Icon = "bi-person-check"
                        }
                    }
                },

                new DocumentTracking
                {
                    Id = 2,
                    TrackingNumber = "DOC-002",
                    DocumentTitle = "Certificate Request",
                    DocumentType = "Certificate",
                    DateReceived = "September 15, 2026",
                    Status = "Completed",
                    Priority = "Normal",
                    Source = "Barangay Office",
                    Sender = "Resident",
                    CurrentLocation = "Barangay Administration Office",
                    CurrentHandler = "Barangay Staff",
                    Description = "Certificate request submitted by a resident.",

                    History = new List<DocumentHistory>
                    {
                        new DocumentHistory
                        {
                            Title = "Request Registered",
                            Description = "The certificate request was registered.",
                            Date = "September 15, 2026",
                            Time = "8:00 AM",
                            Icon = "bi-file-earmark-plus"
                        },

                        new DocumentHistory
                        {
                            Title = "Request Received",
                            Description = "The request was received by the barangay staff.",
                            Date = "September 15, 2026",
                            Time = "8:15 AM",
                            Icon = "bi-inbox"
                        },

                        new DocumentHistory
                        {
                            Title = "Processing Completed",
                            Description = "The requested certificate has been processed successfully.",
                            Date = "September 15, 2026",
                            Time = "10:00 AM",
                            Icon = "bi-check-circle"
                        }
                    }
                },

                new DocumentTracking
                {
                    Id = 3,
                    TrackingNumber = "DOC-003",
                    DocumentTitle = "Barangay Clearance",
                    DocumentType = "Clearance",
                    DateReceived = "September 14, 2026",
                    Status = "Pending",
                    Priority = "Normal",
                    Source = "Barangay Office",
                    Sender = "Resident",
                    CurrentLocation = "Barangay Administration Office",
                    CurrentHandler = "Barangay Staff",
                    Description = "Barangay Clearance request waiting for processing.",

                    History = new List<DocumentHistory>
                    {
                        new DocumentHistory
                        {
                            Title = "Request Submitted",
                            Description = "The resident submitted a Barangay Clearance request.",
                            Date = "September 14, 2026",
                            Time = "2:00 PM",
                            Icon = "bi-file-earmark-plus"
                        },

                        new DocumentHistory
                        {
                            Title = "Request Received",
                            Description = "The request was received by the barangay office.",
                            Date = "September 14, 2026",
                            Time = "2:15 PM",
                            Icon = "bi-inbox"
                        }
                    }
                }
            };


            // IF USER ENTERED A TRACKING NUMBER
            if (!string.IsNullOrWhiteSpace(trackingNumber))
            {
                trackingNumber = trackingNumber.Trim();

                var document = documents.FirstOrDefault(
                    x => x.TrackingNumber.Equals(
                        trackingNumber,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

                if (document != null)
                {
                    ViewBag.Document = document;
                    ViewBag.TrackingNumber = trackingNumber;

                    return View();
                }

                // DOCUMENT NOT FOUND
                ViewBag.Error =
                    "No document was found with tracking number: "
                    + trackingNumber;

                ViewBag.TrackingNumber = trackingNumber;
            }

            return View();
        }


        // =========================================
        // DOCUMENT MODEL
        // =========================================

        public class DocumentTracking
        {
            public int Id { get; set; }

            public string TrackingNumber { get; set; }

            public string DocumentTitle { get; set; }

            public string DocumentType { get; set; }

            public string DateReceived { get; set; }

            public string Status { get; set; }

            public string Priority { get; set; }

            public string Source { get; set; }

            public string Sender { get; set; }

            public string CurrentLocation { get; set; }

            public string CurrentHandler { get; set; }

            public string Description { get; set; }

            public List<DocumentHistory> History { get; set; }
        }


        // =========================================
        // HISTORY MODEL
        // =========================================

        public class DocumentHistory
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public string Date { get; set; }

            public string Time { get; set; }

            public string Icon { get; set; }
        }
    }
}