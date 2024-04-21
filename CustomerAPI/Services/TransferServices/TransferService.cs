using CustomerAPI.ApiServices;
using CustomerAPI.Models.Request;
using CustomerAPI.Models.Response;
using DataBase.Models;
using Domain.SharedModels;
using Domain.TransferDomain;
using System.Security.Cryptography.Xml;

namespace CustomerAPI.Services.TransferServices
{
    public class TransferService : ITransferService
    {
        public readonly IBankTransferService _bankTransferService;
        public readonly ITransferRepository _bankTransferRepository;
        ServiceResponse res = new ServiceResponse();
        string message = "";
        string error = "";

        public TransferService(IBankTransferService bankTransferService,ITransferRepository transferRepository)
        {
            _bankTransferService = bankTransferService;
            _bankTransferRepository = transferRepository;
        }

        public async Task<Tuple<object, string>> TransferRequest(TransferRequest request)
        {
            try
            {
                var mapRequest = MapTransferRequest(request);

                if (mapRequest == null)
                {
                    message = "Sorry, Transfer could not be processed";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                var saveRequest = await _bankTransferRepository.BankTransfer(mapRequest);

                if (saveRequest == null)
                {
                    message = "Sorry, Transfer could not be processed";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    var transfer = await _bankTransferService.InitializeBankTransfer(request);

                    if (transfer.Success)
                    {
                        res.success = true;
                        res.data = transfer;
                        res.message = transfer.Message;
                        return new Tuple<object, string>(res, "success");
                    }
                    else
                    {
                        res.success = false;
                        res.data = null;
                        res.message = transfer.Message;
                        return new Tuple<object, string>(res, "error");
                    }
                }
            }
            catch
            {
                res.success = false;
                res.data = null;
                res.message = "Error occured while processing request";
                return new Tuple<object, string>(res, "error");
            }
            
           
            
        }

        public BankTransfer MapTransferRequest(TransferRequest request)
        {
            return new BankTransfer
            {
                AccountName = request.SenderName,
                AccountNo = request.SenderAccountNumber,
                BeneficiaryAccountName = request.PayeeAccountNumber,
                BeneficiaryAccountNo = request.PayeeAccountNumber,
                DateCreated = DateTime.UtcNow,
                IsSuccessful = true,
                IsReversed = false,
                TransactionStatus = "Pending",
                TransactionRef = request.TransactionReference,
                PlatForm = request.PlatForm,
                Narration = $"Transfer Request from {request.SenderAccountNumber} to {request.PayeeAccountNumber}",
                Id = Guid.NewGuid(),
                ReversalDate = null,
                ApiResponse = null,
                StatusCode = "Pending",
                DebitDate = null,
                ApiUrl = null,
            };
        }

        public async Task<Tuple<object, string>> BankTransferResponse(BankTransferApiResponse response)
        {
            try
            {
                var request = await _bankTransferRepository.UpdateTransaction(response.TransactionRef, response.AccountNumber, response.Status, response.Status);

                if (request)
                {
                    res.success = true;
                    res.data = null;
                    res.message = "Transfer Record Updated";
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    res.success = false;
                    res.data = null;
                    res.message = "Error occured while processing request";
                    return new Tuple<object, string>(res, "error");
                }

            }
            catch
            {
                res.success = false;
                res.data = null;
                res.message = "Error occured while processing request";
                return new Tuple<object, string>(res, "error");
            }
        }
    }
}
