using DataBase.Models;
using Domain.RepositoryDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TransferDomain
{
    public class TransferRepository : ITransferRepository
    {
        private readonly IRepository<BankTransfer> _bankTransferRepository;

        public TransferRepository(IRepository<BankTransfer> bankTransferRepository)
        {
            _bankTransferRepository = bankTransferRepository;
        }

        public async Task<BankTransfer> BankTransfer(BankTransfer bankTransfer)
        {
            try
            {
                await  _bankTransferRepository.AddAsync(bankTransfer);

                return bankTransfer;
            }
            catch
            {
                throw;
            }
        }

        public async Task<BankTransfer> GetBankTransctionByIdAndAccount(string transactionId, string accountNo)
        {
            try
            {
                var transfer = await _bankTransferRepository.GetFirstOrDefaultAsync(x => x.TransactionRef == transactionId);

                if(transfer == null)
                {
                    return null;
                }

                return transfer;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<BankTransfer>> GetBankTransctionsByAccount(int length , string accountNo)
        {
            try
            {
                var transfers = await _bankTransferRepository.GetListAsync(x => x.AccountNo == accountNo);

                var transferList = transfers.ToList();

                return transferList;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateTransaction(string Id, string account, string status, string apiResponse)
        {
            try
            {

                var transfers = await _bankTransferRepository.GetFirstOrDefaultAsync(x => x.AccountNo == account);

                if(transfers == null)
                {
                    return false;
                }

                if (status == "failed")
                {
                    transfers.TransactionStatus = status;
                    transfers.StatusCode = "99";
                    transfers.IsSuccessful = false;
                }
                else
                {
                    transfers.TransactionStatus = status;
                    transfers.StatusCode = "00";
                    transfers.IsSuccessful = true;
                }

                await _bankTransferRepository.UpdateAsync(transfers);

                return true;
         
            }
            catch
            {
                return false;
            }
        }
    }
}
