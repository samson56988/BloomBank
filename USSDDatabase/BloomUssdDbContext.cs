using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSDDatabase.Models;

namespace USSDDatabase
{
    public class BloomUssdDbContext: DbContext
    {
        public BloomUssdDbContext()
        {

        }

        public BloomUssdDbContext(DbContextOptions<BloomUssdDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
