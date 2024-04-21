using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class CableBillsPayment:BaseEntity<Guid>
    {
        public string AccountNo { get; set; }
        public string TranasactionRef { get; set; }
        public string BillType { get; set; }
        public string ServiceProvider { get; set; }
        public decimal Amount { get; set; }
        public DateTime DebitDate { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsSuccessful { get; set; }
        public string Package { get; set; }
        public string APIResponse { get; set; }
        public string ServiceUrl { get; set; }
        public bool IsReversed { get; set; }
        public DateTime ReversalDate { get; set; }
    }
}
