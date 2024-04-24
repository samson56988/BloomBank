using CustomerAPI.Models.Response;
using Microsoft.AspNetCore.SignalR;

namespace CustomerAPI.HubService
{
    public interface IBankTransferHub
    {
        Task SendBankTransferUpdate(TransferResponse transfer);
    }
}
