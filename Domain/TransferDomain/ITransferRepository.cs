using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TransferDomain
{
    public interface ITransferRepository
    {
        Task<BankTransfer> BankTransfer(BankTransfer bankTransfer);

        Task<BankTransfer> GetBankTransctionByIdAndAccount(string transactionId,string accountNo);

        Task<List<BankTransfer>> GetBankTransctionsByAccount(int length, string accountNo);
        Task<bool> UpdateTransaction(string Id, string account, string status, string apiResponse);

    }
}
