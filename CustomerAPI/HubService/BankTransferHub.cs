using CustomerAPI.Models.Response;
using Microsoft.AspNetCore.SignalR;

namespace CustomerAPI.HubService
{
    public class BankTransferHub:Hub
    {
        private static readonly Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();

        public async Task SendBankTransferUpdate(TransferResponse transfer)
        {
            if (string.IsNullOrEmpty(transfer.PayeeAccountNumber))
            {
                throw new ArgumentNullException(nameof(transfer.PayeeAccountNumber), "User ID cannot be null or empty.");
            }

            var connectionId = ConnectedUsers.FirstOrDefault(x => x.Value == transfer.PayeeAccountNumber || x.Value == transfer.PayeeAccountNumber).Key;

            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveBankTransferUpdate", transfer);
            }
            else
            {
                Console.WriteLine($"Bank Transfer Response Failed");
            }
        }


        public override async Task OnConnectedAsync()
        {
            string userId = Context.User.Identity.Name; 
            string connectionId = Context.ConnectionId;

            if (!ConnectedUsers.ContainsKey(connectionId))
            {
                ConnectedUsers.Add(connectionId, userId);
            }

            await Clients.All.SendAsync("UserConnected", userId);
            await base.OnConnectedAsync();
        }
    }
}
