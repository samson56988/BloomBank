using EmailProcessor.EmailServices;
using MiddleWareAPI.DatabaseLogic;
using MiddleWareDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService
{
    public class TransferProcessor
    {
        private readonly IDatabaseLogic _databaseLogic;
        private readonly IEmailService _emailService;

        public TransferProcessor(IDatabaseLogic databaseLogic,IEmailService emailService)
        {
            _databaseLogic = databaseLogic;
            _emailService = emailService;
        }


        public async Task<BloomCustomer> GetAccountDetails(string accountId)
        {
            var accounts = await _databaseLogic.GetCustomerByAccountNumber(accountId);

            return accounts;
        }


        public async Task<bool> Transfer(string senderAccount, string beneficiaryAccountNo, decimal amount, string narration)
        {
            var processor = await _databaseLogic.DebitCustomer(senderAccount, beneficiaryAccountNo, amount,narration);

            return processor;
        }

        public async Task<bool> SendTransactionNotification(string recipientName, string recipientEmail, string transactionDate, string transactionType, string transactionDescription, string amount, bool IsSuccess)
        {
            var processor = await _emailService.BankTransferNotification(recipientName,recipientEmail,transactionDate,transactionType,transactionDescription,amount,IsSuccess);

            return processor;
        }


    }
}
