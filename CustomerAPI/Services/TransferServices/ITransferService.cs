using CustomerAPI.Models.Request;
using CustomerAPI.Models.Response;

namespace CustomerAPI.Services.TransferServices
{
    public interface ITransferService
    {
        Task<Tuple<object, string>> TransferRequest(TransferRequest request);

        Task<Tuple<object, string>> BankTransferResponse(BankTransferApiResponse response);
    }
}
