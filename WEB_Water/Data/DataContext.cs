using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Data
{
    public class DataContext :DbContext
    {
        public DbSet<Customer> Customers { get; set; } // Property responsible for the table

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Reading> Readings { get; set; }

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }
    }
}
