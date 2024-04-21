using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class BloomDatabaseContext:DbContext
    {
        public BloomDatabaseContext()
        {

        }

        public BloomDatabaseContext(DbContextOptions<BloomDatabaseContext> options) : base(options) 
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<AuditTrails> AuditTrails { get; set; }
        public DbSet<AirtimePurchase> AirtimePurchase { get; set;}
        public DbSet<DataPurchase> DataPurchase { get; set; }
        public DbSet<CustomerTransactionToken> CustomerTransactionTokens { get; set; }
        public DbSet<BankTransfer> BankTransfers { get; set; }
        public DbSet<OtherBankTransfers> OtherBankTransfers { get; set;}
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<UtitlityBillPayment> UtitlityBillPayments { get; set; }
        public DbSet<CableBillsPayment> CableBillsPayments { get;set; }
    }
}
