using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class OtherBankTransfers:BaseEntity<Guid>
    {
        public string AccountNo { get; set; }
        public int BankCode { get; set; }
        public string BankName { get; set; }
        public string TransactionRef { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DebitDate { get; set; }
        public string ApiResponse { get; set; }
        public string DuneTransactionReference {  get; set; }
        public string DuneApiResponse { get; set; }
        public string TransactionStatus { get; set; }
        public string StatusCode { get; set; }
        public string ApiUrl { get; set; }
        public bool IsSuccessful { get; set; }
        public bool IsReversed { get; set; }
        public DateTime? ReversalDate { get; set; }   
    }
}
