using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EmailProcessor.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IMailJetService _mailJetService;


        public EmailService(IMailJetService mailJetService)
        {
            _mailJetService = mailJetService;
        }

        public async Task<bool> AccountCreationNotification(string recipientName, string recipientEmail, string accountNumber, string bankName)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "AccountCreationNotification.txt");

                string mailBody = await GenerateEmailBodyForAccountCreation(filePath, recipientName, accountNumber, bankName);

                string senderEmail = ConfigurationManager.AppSettings["EmailAPISender"];

                var sendEmail = _mailJetService.SendEmailAsync(recipientEmail, senderEmail, mailBody, "Account Opening Notification");

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> GenerateEmailBodyForAccountCreation(string filePath, string recipientName, string accountNumber, string bankName)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string mailText = await reader.ReadToEndAsync();

                var keys = new Dictionary<string, string>
                {
                    { "{RecipientName}", recipientName },
                    { "{AccountNumber}", accountNumber },
                    { "{BankName}", bankName }
                };

                StringBuilder sb = new StringBuilder(mailText);

                foreach (var key in keys)
                {
                    sb.Replace(key.Key, key.Value ?? string.Empty);
                }

                return sb.ToString();
            }
        }

        public async Task<bool> BankTransferNotification(string recipientName, string recipientEmail, string transactionDate, string transactionType, string transactionDescription, string amount, bool IsSuccess)
        {
            try
            {
                string templateFileName = IsSuccess ? "BankTransferSuccess.txt" : "BankTransferFailed.txt";

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", templateFileName);

                string mailBody = await GenerateEmailBodyForBankTransfer(filePath, recipientName, transactionDate, transactionType, transactionDescription, amount);

                string senderEmail = ConfigurationManager.AppSettings["EmailAPISender"];

                var sendEmail = _mailJetService.SendEmailAsync("samson56982@gmail.com", senderEmail, mailBody, "Bank Transfer");

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> GenerateEmailBodyForBankTransfer(string filePath, string recipientName, string transactionDate, string transactionType, string transactionDescription, string amount)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string mailText = await reader.ReadToEndAsync();

                var keys = new Dictionary<string, string>
                {
                    { "{recipientName}", recipientName },
                    { "{TransactionDate}", transactionDate },
                    { "{transactionType}", transactionType },
                    { "{TransactionDescription}", transactionDescription },
                    { "{TransactionAmount}", amount }
                };

                StringBuilder sb = new StringBuilder(mailText);

                foreach (var key in keys)
                {
                    sb.Replace(key.Key, key.Value ?? string.Empty);
                }

                return sb.ToString();
            }
        }


    }
}
