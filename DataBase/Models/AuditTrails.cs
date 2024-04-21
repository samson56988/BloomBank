using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class AuditTrails:BaseEntity<Guid>
    {
        public string Action {  get; set; }
        public string Ip { get; set; }
        public string IsUser { get; set; }
        public string UserIdentification { get; set; }
        public string Type { get; set; }
        public string DateCreated { get; set; }
        public bool IsSuccess { get; set; }
    }
}
