
using BarangayDocumentTracking.Models;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text.Encodings.Web;

namespace BarangayDocumentTracking.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendRequestConfirmationAsync(
            DocumentRequest request,
            Document document)
        {
            var settings = _configuration.GetSection("EmailSettings");

            string host = settings["Host"] ?? "";
            int port = int.Parse(settings["Port"] ?? "587");
            string username = settings["Username"] ?? "";
            string password = settings["Password"] ?? "";
            string fromEmail = settings["FromEmail"] ?? "";
            string fromName = settings["FromName"] ?? "Barangay Document Tracking System";

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fromEmail))
            {
                throw new InvalidOperationException(
                    "Email settings are incomplete. Please configure SMTP credentials.");
            }

            // Create email message
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(fromName, fromEmail));

            message.To.Add(
                new MailboxAddress(request.ResidentName, request.Email));

            message.Subject = "Document Request Submitted Successfully";

            // Safely encode resident-provided values for HTML
            string residentName = HtmlEncoder.Default.Encode(request.ResidentName);
            string requestNumber = HtmlEncoder.Default.Encode(request.RequestNumber);
            string documentName = HtmlEncoder.Default.Encode(document.DocumentName);
            string purpose = HtmlEncoder.Default.Encode(request.Purpose);
            string status = HtmlEncoder.Default.Encode(request.Status);

            string requestDate = request.RequestDate.ToString(
                "MMMM dd, yyyy hh:mm tt");

            string fee = document.Fee == 0
                ? "FREE"
                : "₱" + document.Fee.ToString("N2");

            // Email content
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = $@"
            <!DOCTYPE html>
            <html>
            <body style='font-family: Arial, sans-serif;
                         background-color: #f4f6f9;
                         padding: 25px;'>

                <div style='max-width: 600px;
                            margin: auto;
                            background: #ffffff;
                            border-radius: 10px;
                            overflow: hidden;
                            border: 1px solid #e5e7eb;'>

                    <div style='background: #2c3e50;
                                color: white;
                                padding: 25px;
                                text-align: center;'>

                        <h2 style='margin: 0;'>
                            Barangay Document Tracking System
                        </h2>

                        <p style='margin-bottom: 0;'>
                            Document Request Confirmation
                        </p>
                    </div>

                    <div style='padding: 30px;'>

                        <h3 style='color: #198754;'>
                            Request Submitted Successfully!
                        </h3>

                        <p>Dear {residentName},</p>

                        <p>
                            Thank you for submitting your document request.
                            Your request has been successfully recorded
                            in our system.
                        </p>

                        <div style='background: #f8f9fa;
                                    padding: 20px;
                                    border-radius: 8px;
                                    margin: 20px 0;'>

                            <h3 style='margin-top: 0;
                                       color: #2c3e50;'>
                                Request Information
                            </h3>

                            <p><strong>Request Number:</strong>
                                {requestNumber}</p>

                            <p><strong>Document:</strong>
                                {documentName}</p>

                            <p><strong>Purpose:</strong>
                                {purpose}</p>

                            <p><strong>Document Fee:</strong>
                                {fee}</p>

                            <p><strong>Date Submitted:</strong>
                                {requestDate}</p>

                            <p><strong>Status:</strong>
                                <span style='color: #d97706;
                                             font-weight: bold;'>
                                    {status}
                                </span>
                            </p>

                        </div>

                        <p>
                            Please keep your request number for future
                            reference and tracking.
                        </p>

                        <p>
                            Thank you!
                        </p>

                        <p>
                            <strong>Barangay Administration</strong><br/>
                            Barangay Document Tracking System
                        </p>

                    </div>

                    <div style='background: #f1f3f5;
                                padding: 15px;
                                text-align: center;
                                color: #6c757d;
                                font-size: 12px;'>

                        This is an automated notification.
                        Please do not reply directly to this email.

                    </div>

                </div>

            </body>
            </html>";

            message.Body = bodyBuilder.ToMessageBody();

            // Send email using Gmail SMTP
            using var client = new SmtpClient();

            await client.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                username,
                password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
