using System;
using System.Net;
using System.Net.Mail;

class Program
{
    static void Main()
    {
        SendEmail();
    }

    static void SendEmail()
    {
        try
        {
            string smtpHost = "smtp.office365.com";
            int smtpPort = 587;
            string smtpUser = "hr@credentinfotech.com";
            string smtpPass = "Non84273";  // Do not hardcode passwords in production
            string fromEmail = "hr@credentinfotech.com";
            string toEmail = "chhagan.sinha@credentinfotech.com"; // Change this

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Test Email from C#",
                Body = "This is a test email sent using C# and System.Net.Mail.",
                IsBodyHtml = false
            };
            mail.To.Add(toEmail);

            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true; // Office 365 requires SSL/TLS
                smtpClient.Send(mail);
            }

            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}
