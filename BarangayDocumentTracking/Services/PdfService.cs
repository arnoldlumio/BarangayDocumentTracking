using System;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using BarangayDocumentTracking.Models;

namespace BarangayDocumentTracking.Services
{
    public class PdfService
    {
        private readonly IWebHostEnvironment _environment;

        public PdfService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public byte[] GenerateOutgoingDocument(
            OutgoingDocument document)
        {
            // ==========================================
            // QUESTPDF LICENSE
            // ==========================================

            QuestPDF.Settings.License =
                LicenseType.Community;


            // ==========================================
            // LOAD BARANGAY LOGO
            // ==========================================

            string logoPath = Path.Combine(
                _environment.WebRootPath,
                "img",
                "barangay_logo.png"
            );

            byte[] logoBytes = Array.Empty<byte>();

            if (File.Exists(logoPath))
            {
                logoBytes = File.ReadAllBytes(logoPath);
            }


            // ==========================================
            // CREATE PDF
            // ==========================================

            var pdf = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    // ==================================
                    // PAGE SETTINGS
                    // ==================================

                    page.Size(PageSizes.A4);

                    // Use simple Margin()
                    // to avoid version-specific
                    // QuestPDF Margin overload errors.
                    page.Margin(45);

                    // IMPORTANT:
                    // Do NOT use Times New Roman,
                    // Arial, or other system fonts.
                    //
                    // QuestPDF 2026.9+ does not use
                    // system fonts by default.
                    //
                    // We intentionally do not specify
                    // FontFamily() here.
                    //
                    // QuestPDF will use its available
                    // bundled/default font.

                    page.DefaultTextStyle(
                        x => x
                            .FontSize(10)
                    );


                    // ==================================
                    // HEADER
                    // ==================================

                    page.Header()
                        .Column(header =>
                        {
                            // --------------------------------
                            // LOGOS + GOVERNMENT HEADER
                            // --------------------------------

                            header.Item()
                                .Row(row =>
                                {
                                    // =================================
                                    // LEFT LOGO
                                    // =================================

                                    row.ConstantItem(65)
                                        .Height(65)
                                        .AlignMiddle()
                                        .AlignCenter()
                                        .Element(container =>
                                        {
                                            if (logoBytes.Length > 0)
                                            {
                                                container
                                                    .Image(logoBytes)
                                                    .FitArea();
                                            }
                                        });


                                    // =================================
                                    // CENTER HEADER
                                    // =================================

                                    row.RelativeItem()
                                        .AlignCenter()
                                        .Column(column =>
                                        {
                                            column.Item()
                                                .AlignCenter()
                                                .Text(
                                                    "REPUBLIC OF THE PHILIPPINES"
                                                )
                                                .FontSize(9)
                                                .Bold();


                                            column.Item()
                                                .AlignCenter()
                                                .Text(
                                                    "BARANGAY OFFICE"
                                                )
                                                .FontSize(19)
                                                .Bold();


                                            column.Item()
                                                .AlignCenter()
                                                .Text(
                                                    "Barangay Document Tracking System"
                                                )
                                                .FontSize(9);


                                            column.Item()
                                                .PaddingTop(3)
                                                .AlignCenter()
                                                .Text(
                                                    "OFFICIAL DOCUMENT"
                                                )
                                                .FontSize(8)
                                                .Bold();
                                        });


                                    // =================================
                                    // RIGHT LOGO
                                    // =================================

                                    row.ConstantItem(65)
                                        .Height(65)
                                        .AlignMiddle()
                                        .AlignCenter()
                                        .Element(container =>
                                        {
                                            if (logoBytes.Length > 0)
                                            {
                                                container
                                                    .Image(logoBytes)
                                                    .FitArea();
                                            }
                                        });
                                });


                            // --------------------------------
                            // HEADER LINE
                            // --------------------------------

                            header.Item()
                                .PaddingTop(8)
                                .LineHorizontal(1.5f);
                        });


                    // ==================================
                    // MAIN CONTENT
                    // ==================================

                    page.Content()
                        .PaddingTop(25)
                        .Column(column =>
                        {
                            column.Spacing(14);


                            // =================================
                            // DOCUMENT TITLE
                            // =================================

                            column.Item()
                                .AlignCenter()
                                .Text("OUTGOING DOCUMENT")
                                .Bold()
                                .FontSize(17);


                            column.Item()
                                .AlignCenter()
                                .Text(
                                    "Official Record of Released Document"
                                )
                                .FontSize(9)
                                .FontColor("#666666");


                            // =================================
                            // DOCUMENT NUMBER
                            // =================================

                            column.Item()
                                .PaddingTop(5)
                                .AlignRight()
                                .Text(text =>
                                {
                                    text.Span("Document No.: ")
                                        .Bold();

                                    text.Span(
                                        document.DocumentNumber
                                    );
                                });


                            // =================================
                            // DOCUMENT INFORMATION
                            // =================================

                            column.Item()
                                .PaddingTop(5)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(145);

                                        columns.RelativeColumn();
                                    });


                                    // ---------------------------------
                                    // DOCUMENT NUMBER
                                    // ---------------------------------

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Background("#F2F2F2")
                                        .Padding(8)
                                        .Text("Document Number")
                                        .Bold();

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Padding(8)
                                        .Text(
                                            document.DocumentNumber
                                        );


                                    // ---------------------------------
                                    // RECIPIENT
                                    // ---------------------------------

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Background("#F2F2F2")
                                        .Padding(8)
                                        .Text("Recipient")
                                        .Bold();

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Padding(8)
                                        .Text(
                                            document.Recipient
                                        );


                                    // ---------------------------------
                                    // SUBJECT
                                    // ---------------------------------

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Background("#F2F2F2")
                                        .Padding(8)
                                        .Text("Subject")
                                        .Bold();

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Padding(8)
                                        .Text(
                                            document.Subject
                                        );


                                    // ---------------------------------
                                    // DATE RELEASED
                                    // ---------------------------------

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Background("#F2F2F2")
                                        .Padding(8)
                                        .Text("Date Released")
                                        .Bold();

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Padding(8)
                                        .Text(
                                            document.DateReleased
                                                .ToString(
                                                    "MMMM dd, yyyy"
                                                )
                                        );


                                    // ---------------------------------
                                    // STATUS
                                    // ---------------------------------

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Background("#F2F2F2")
                                        .Padding(8)
                                        .Text("Status")
                                        .Bold();

                                    table.Cell()
                                        .Border(1)
                                        .BorderColor("#D0D0D0")
                                        .Padding(8)
                                        .Text(
                                            document.Status
                                        )
                                        .Bold();
                                });


                            // =================================
                            // DOCUMENT DETAILS
                            // =================================

                            column.Item()
                                .PaddingTop(15)
                                .Text("DOCUMENT DETAILS")
                                .Bold()
                                .FontSize(12);


                            column.Item()
                                .PaddingTop(3)
                                .LineHorizontal(1);


                            column.Item()
                                .PaddingTop(8)
                                .Text(
                                    "This document is officially released " +
                                    "by the Barangay Office and has been " +
                                    "recorded in the Barangay Document " +
                                    "Tracking System."
                                )
                                .LineHeight(1.5f);


                            column.Item()
                                .Text(
                                    "The information presented in this " +
                                    "document corresponds to the outgoing " +
                                    "document record maintained by the " +
                                    "barangay office."
                                )
                                .LineHeight(1.5f);


                            // =================================
                            // OFFICIAL NOTICE
                            // =================================

                            column.Item()
                                .PaddingTop(12)
                                .Border(1)
                                .BorderColor("#CCCCCC")
                                .Background("#F8F8F8")
                                .Padding(10)
                                .Column(notice =>
                                {
                                    notice.Item()
                                        .Text("OFFICIAL NOTICE")
                                        .Bold()
                                        .FontSize(10);


                                    notice.Item()
                                        .PaddingTop(4)
                                        .Text(
                                            "This document is generated " +
                                            "electronically from the " +
                                            "Barangay Document Tracking " +
                                            "System."
                                        )
                                        .FontSize(9);
                                });


                            // =================================
                            // SIGNATURE
                            // =================================

                            column.Item()
                                .PaddingTop(50)
                                .AlignRight()
                                .Column(signature =>
                                {
                                    signature.Item()
                                        .AlignCenter()
                                        .Text(
                                            "____________________________"
                                        )
                                        .Bold();


                                    signature.Item()
                                        .AlignCenter()
                                        .Text(
                                            "Barangay Secretary"
                                        )
                                        .Bold();


                                    signature.Item()
                                        .AlignCenter()
                                        .Text(
                                            "Authorized Signatory"
                                        )
                                        .FontSize(9);
                                });


                            // =================================
                            // RECORD INFORMATION
                            // =================================

                            column.Item()
                                .PaddingTop(35)
                                .LineHorizontal(1);


                            column.Item()
                                .PaddingTop(5)
                                .Row(row =>
                                {
                                    // LEFT
                                    row.RelativeItem()
                                        .Text(text =>
                                        {
                                            text.Span("Document ID: ")
                                                .Bold();

                                            text.Span(
                                                document.Id.ToString()
                                            );
                                        });


                                    // RIGHT
                                    row.RelativeItem()
                                        .AlignRight()
                                        .Text(text =>
                                        {
                                            text.Span("Status: ")
                                                .Bold();

                                            text.Span(
                                                document.Status
                                            );
                                        });
                                });
                        });


                    // ==================================
                    // FOOTER
                    // ==================================

                    page.Footer()
                        .PaddingTop(10)
                        .Column(footer =>
                        {
                            // FOOTER LINE

                            footer.Item()
                                .LineHorizontal(1);


                            // FOOTER CONTENT

                            footer.Item()
                                .PaddingTop(5)
                                .Row(row =>
                                {
                                    // LEFT

                                    row.RelativeItem()
                                        .Text(
                                            "Barangay Document Tracking System"
                                        )
                                        .FontSize(8)
                                        .FontColor("#666666");


                                    // RIGHT

                                    row.RelativeItem()
                                        .AlignRight()
                                        .Text(
                                            "Generated: " +
                                            DateTime.Now.ToString(
                                                "MMM dd, yyyy hh:mm tt"
                                            )
                                        )
                                        .FontSize(8)
                                        .FontColor("#666666");
                                });
                        });
                });
            });


            // ==========================================
            // RETURN GENERATED PDF
            // ==========================================

            return pdf.GeneratePdf();
        }
    }
}