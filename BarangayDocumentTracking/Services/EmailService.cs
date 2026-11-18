
using BarangayDocumentTracking.Models;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

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
            // 1. Read Gmail SMTP settings
            var settings = _configuration.GetSection("EmailSettings");

            string host = settings["Host"] ?? "";
            string username = settings["Username"] ?? "";
            string password = settings["Password"] ?? "";
            string fromEmail = settings["FromEmail"] ?? "";

            string fromName = settings["FromName"]
                ?? "Barangay Document Tracking System";

            if (!int.TryParse(settings["Port"], out int port))
            {
                throw new InvalidOperationException(
                    "Invalid SMTP port configuration.");
            }

            // 2. Validate SMTP settings
            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fromEmail))
            {
                throw new InvalidOperationException(
                    "Email SMTP settings are incomplete. Check your secrets.json.");
            }

            if (!MailboxAddress.TryParse(request.Email, out var recipient))
            {
                throw new InvalidOperationException(
                    "Invalid resident email address.");
            }

            // 3. Create email message
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(fromName, fromEmail));

            // Send email to resident's submitted email address
            message.To.Add(
                new MailboxAddress(
                    request.ResidentName,
                    recipient.Address));

            message.Subject =
                "Document Request Confirmation - " +
                request.RequestNumber;

            // 4. Encode information for HTML safety
            string residentName =
                HtmlEncoder.Default.Encode(request.ResidentName);

            string requestNumber =
                HtmlEncoder.Default.Encode(request.RequestNumber);

            string documentName =
                HtmlEncoder.Default.Encode(document.DocumentName);

            string purpose =
                HtmlEncoder.Default.Encode(request.Purpose);

            string status =
                HtmlEncoder.Default.Encode(request.Status);

            string requestDate =
                request.RequestDate.ToString("MMMM dd, yyyy hh:mm tt");

            string fee = document.Fee == 0
                ? "FREE"
                : "PHP " + document.Fee.ToString("N2");

            // 5. Create email body
            var body = new BodyBuilder();

            body.TextBody = $@"
BARANGAY DOCUMENT TRACKING SYSTEM

Hello {request.ResidentName},

Your document request has been successfully submitted.

REQUEST INFORMATION

Request Number: {request.RequestNumber}
Document: {document.DocumentName}
Purpose: {request.Purpose}
Document Fee: {fee}
Date Submitted: {requestDate}
Status: {request.Status}

Please keep your request number for future reference and tracking.

Thank you!

Barangay Administration
Barangay Document Tracking System
";

            body.HtmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Document Request Confirmation</title>
</head>

<body style='
    margin:0;
    padding:25px;
    background-color:#f4f6f9;
    font-family:Arial,sans-serif;
'>

    <div style='
        max-width:600px;
        margin:auto;
        background:#ffffff;
        border:1px solid #dddddd;
        border-radius:10px;
        overflow:hidden;
    '>

        <div style='
            background:#2c3e50;
            color:white;
            padding:25px;
            text-align:center;
        '>
            <h2>Barangay Document Tracking System</h2>
            <p>Document Request Confirmation</p>
        </div>

        <div style='padding:30px;'>

            <h2 style='color:#198754;'>
                Request Submitted Successfully!
            </h2>

            <p>Dear <strong>{residentName}</strong>,</p>

            <p>
                Thank you for submitting your document request.
                Your request has been successfully recorded
                in our system.
            </p>

            <div style='
                background:#f8f9fa;
                padding:20px;
                border-radius:8px;
            '>

                <h3>Request Information</h3>

                <p>
                    <strong>Request Number:</strong>
                    {requestNumber}
                </p>

                <p>
                    <strong>Document:</strong>
                    {documentName}
                </p>

                <p>
                    <strong>Purpose:</strong>
                    {purpose}
                </p>

                <p>
                    <strong>Document Fee:</strong>
                    {fee}
                </p>

                <p>
                    <strong>Date Submitted:</strong>
                    {requestDate}
                </p>

                <p>
                    <strong>Status:</strong>
                    <span style='
                        color:#d97706;
                        font-weight:bold;
                    '>
                        {status}
                    </span>
                </p>

            </div>

            <p style='margin-top:25px;'>
                Please keep your request number for future
                reference and tracking.
            </p>

            <p>Thank you!</p>

            <p>
                <strong>Barangay Administration</strong><br>
                Barangay Document Tracking System
            </p>

        </div>

        <div style='
            background:#f1f3f5;
            padding:15px;
            text-align:center;
            color:#777777;
            font-size:12px;
        '>
            This is an automated email notification.
            Please do not reply directly to this message.
        </div>

    </div>

</body>
</html>";

            message.Body = body.ToMessageBody();

            // 6. Connect to Gmail SMTP and send email
            using var client = new SmtpClient();

            client.Timeout = 30000;

            await client.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls
            );

            try
            {
                // Authenticate Gmail account
                await client.AuthenticateAsync(
                    username,
                    password
                );

                // Send confirmation email
                await client.SendAsync(message);
            }
            finally
            {
                // Disconnect even if sending fails
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
