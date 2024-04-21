using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class CustomerTransactionToken:BaseEntity<Guid>
    {
        public string AccountNo { get; set; }
        public string Token {  get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Updated {  get; set; }
    }
}
