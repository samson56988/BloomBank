using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailProcessor.EmailServices
{
    public interface IMailJetService
    {
        Task<bool> SendEmailAsync(string recipientEmail, string senderEmail, string mailBody, string subject);
    }

    public class MailJetService : IMailJetService
    {
        
        public async Task<bool> SendEmailAsync(string recipientEmail, string senderEmail, string mailBody, string subject)
        {
            try
            {
                string publicKey = ConfigurationManager.AppSettings["EmailAPIKey"];
                string privateKey = ConfigurationManager.AppSettings["EmailAPISecret"];

                MailjetClient client = new MailjetClient(publicKey, privateKey);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.FromEmail, senderEmail)
                .Property(Send.Recipients, new JArray {
                new JObject {
                    {"Email", recipientEmail}
                }
                })
                .Property(Send.Subject, subject)
                .Property(Send.HtmlPart, mailBody);

                MailjetResponse response = await client.PostAsync(request);

                if(response.IsSuccessStatusCode)
                {
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    return response.IsSuccessStatusCode;
                }

            }
            catch
            {
                return false;
            }
        }
    }
}
