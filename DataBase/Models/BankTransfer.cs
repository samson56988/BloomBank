using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class BankTransfer : BaseEntity<Guid>
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string TransactionRef {  get; set; }
        public string BeneficiaryAccountNo { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public string PlatForm { get; set; }
        public string Narration { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DebitDate { get; set; }
        public string ApiResponse { get; set; }
        public string TransactionStatus { get; set; }
        public string StatusCode { get; set; }
        public string ApiUrl { get; set; }
        public bool IsSuccessful { get; set; }
        public bool IsReversed { get; set; }
        public DateTime? ReversalDate {  get; set; }
    }
}
