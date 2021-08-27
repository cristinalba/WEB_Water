using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Data
{
    public class SeedDb
    {
        private readonly DataContext _context; //join DB
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); //If there is no DB created, this will create one

          
            if (!_context.Customers.Any())
            {
                AddCustomer("Juan", "Martin");
                await _context.SaveChangesAsync(); //Save data in the new Table
            }
            if (!_context.Addresses.Any())
            {
                AddAddress("Rua X", "0000-000", "Lisboa", DateTime.Today, "Millenium");
                await _context.SaveChangesAsync(); //Save data in the new Table
            }

        }

        private void AddAddress(string street, string code, string city, DateTime date, string bank)
        {
            _context.Addresses.Add(new Address
            {
                AddressName = street,
                PostalCode = code,
                City = city,
                BeginOfContract = date,
                NameOfBank = bank
            });
        }

        private void AddCustomer(string first, string last)
        {
            _context.Customers.Add(new Customer
            {
                FirstName = first,
                LastName = last,
                NIF_customer = _random.Next(100000000),
                Telefone = _random.Next(900000000),
                Email = "kamistesta@gmail.com"
            });
        }
    }
}
