using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class DataPurchase:BaseEntity<Guid>
    {
        public string AccountNo { get; set; }
        public decimal Amount { get; set; }
        public string Telcos { get; set; }
        public string DataPlan { get; set; }
        public bool IsSuccessful { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string TransactionReference { get; set; }
        public string APIResponse { get; set; }
        public DateTime Created { get; set; }
        public DateTime DebitDate { get; set; }
        public bool IsReversed { get; set; }
    }
}
