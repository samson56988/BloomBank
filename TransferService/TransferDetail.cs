using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService
{
    public class TransferDetail
    {
        public Guid Id { get; set; }
        public string TransactionReference { get; set; }
        public string SenderName { get; set; }
        public string SenderAccountNumber { get; set; }
        public string PayeeAccountNumber { get; set; }
        public string PayeeName { get; set; }
        public DateTime TransferDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string TransferStatus { get; set; }
        public string Narration { get; set; }
        public string StatusFlag { get; set; }
        public DateTime DateDebited { get; set; }
        public string PlatForm { get; set; }
        public decimal Amount { get; set; }
    }
}
