using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Data
{
    public class DataContext : IdentityDbContext<User>
    {
      
        public DbSet<Reader> Readers { get; set; }// Property responsible for the table

        public DbSet<Reading> Readings { get; set; }

        public DbSet<Bill> Bills { get; set; }

        public DbSet<Consumption> Consumptions { get; set; }



        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }
    }
}
