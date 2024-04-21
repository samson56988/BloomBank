using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SharedModels
{
    public class ServiceResponse
    {
        public Boolean success { get; set; }

        public String message { get; set; }

        public Int32 dataCount { get; set; }

        public Object data { get; set; }

        public Tuple<string, decimal> tuple { get; set; }
    }
}
