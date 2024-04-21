using DataBase.Models;
using Domain.SharedModels;
using MiddleWareAPI.DatabaseLogic;
using MiddleWareAPI.Models;
using MiddleWareAPI.Models.Dto;
using MiddleWareDomain.Models;

namespace MiddleWareAPI.Services.TransferService
{
    public interface ITransferService
    {
        Task<Tuple<object, string>> InitiateBankTransfer(InitializeBankTransfer transfer);
    }

    public class TransferService : ITransferService
    {
        private readonly IDatabaseLogic _databaseLogic;
        ServiceResponse res = new ServiceResponse();
        string message = "";
        string error = "";

        public TransferService(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        public async Task<Tuple<object, string>> InitiateBankTransfer(InitializeBankTransfer request)
        {
            try
            {
                var validateSender = await _databaseLogic.GetCustomerByAccountNumber(request.SenderAccountNumber);
                if (validateSender == null)
                {
                    message = "Invalid Account Provided";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                if (request.Amount > validateSender.TransactionLimit)
                {
                    message = "Amout Exceed Transfer Limits";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                DateTime dateTime = DateTime.Now;
                var validateDailyLimit = await _databaseLogic.GetTotalSuccessfulTransactionAmountForDay(request.SenderAccountNumber, dateTime);
                if (validateDailyLimit + request.Amount >  validateSender.TransactionLimit)
                {
                    message = "Amout Exceed Transaction Limits";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                var mapper = MapToBankTransfer(request);

                var transfer = await _databaseLogic.InitiateBankTransfer(mapper);
                if (transfer)
                {
                    message = "Transaction Processing";
                    res.success = true;
                    res.data = request;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch 
            {
                message = "Error processing request";
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");

            }
        }

        public  BankTransfers MapToBankTransfer(InitializeBankTransfer initializeBankTransfer)
        {
            return new BankTransfers
            {
                Id = Guid.NewGuid(),
                TransactionReference = initializeBankTransfer.TransactionReference,
                SenderName = initializeBankTransfer.SenderName,
                SenderAccountNumber = initializeBankTransfer.SenderAccountNumber,
                PayeeAccountNumber = initializeBankTransfer.PayeeAccountNumber,
                PayeeName = initializeBankTransfer.PayeeName,
                TransferDate = DateTime.UtcNow, 
                IsSuccessful = false, 
                TransferStatus = "Pending", 
                Narration = initializeBankTransfer.Narration,
                StatusFlag = "0", 
                PlatForm = initializeBankTransfer.PlatForm,
                Amount = initializeBankTransfer.Amount
            };
        }
    }
}
