using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCaller
{
    internal class ApiResponse
    {
    }


    public class Rootobject
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int dataCount { get; set; }
        public Data data { get; set; }
        public object tuple { get; set; }
    }

    public class Data
    {
        public string accountNo { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string bvn { get; set; }
        public string accountType { get; set; }
        public string identificationType { get; set; }
        public string identificationNumber { get; set; }
        public int transactionLimit { get; set; }
        public DateTime dateOnboarded { get; set; }
        public int accountBalance { get; set; }
        public string cifNo { get; set; }
        public DateTime dateOfBirth { get; set; }
        public bool hasPND { get; set; }
        public string currencyCode { get; set; }
        public string accountName { get; set; }
    }

}
