using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmailProcessor.EmailServices
{
    public interface IEmailService
    {
        Task<bool> BankTransferNotification(string recipientName, string recipientEmail, string transactionDate, string transactionType, string transactionDescription, string amount, bool IsSuccess);

        Task<bool> AccountCreationNotification(string recipientName, string recipientEmail, string accountNumber, string bankName);
    }
}
