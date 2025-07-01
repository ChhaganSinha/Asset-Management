namespace AssetManagement.Server.EmailService
{
    public interface IMailService
    {
        Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
    }
}
