using AssetManagement.Dto;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Mail;

namespace AssetManagement.Server.EmailService
{
    public class MailService : IMailService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _fromEmail;

        public MailService(IConfiguration configuration)
        {
            var clientId = configuration["AzureAD:ClientId"];
            var clientSecret = configuration["AzureAD:ClientSecret"];
            var tenantId = configuration["AzureAD:TenantId"];
            _fromEmail = configuration["MailSettings:FromEmail"];

            _graphClient = CreateGraphClient(clientId, clientSecret, tenantId);
        }

        private GraphServiceClient CreateGraphClient(string clientId, string clientSecret, string tenantId)
        {
            var confidentialClientApp = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();

            var authProvider = new DelegateAuthenticationProvider(async (requestMessage) =>
            {
                var authResult = await confidentialClientApp.AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            });

            return new GraphServiceClient(authProvider);
        }

        public async Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
        {
            var emailMessage = new Message
            {
                Subject = Subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = HTMLBody
                },
                ToRecipients = new List<Recipient>
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = ToEmail
                        }
                    }
                }
            };

            _graphClient.Users[_fromEmail].SendMail(emailMessage, true).Request().PostAsync().Wait();

            //await _graphClient.Users[_fromEmail].SendMail.PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
            //{
            //    Message = emailMessage,
            //    SaveToSentItems = true
            //});
        }
    }
}
