using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService.Models
{
    public class BankTransferResponse
    {
        public string TransactionRef { get; set; }
        public string AccountNumber { get; set; }
        public string ApiResponse { get; set; }
        public string Status { get; set; }
    }
}
