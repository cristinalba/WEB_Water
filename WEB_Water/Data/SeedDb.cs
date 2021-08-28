using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;

namespace WEB_Water.Data
{
    public class SeedDb
    {
        private readonly DataContext _context; //join DB
        private Random _random;
        private readonly IUserHelper _userHelper;
        //private readonly UserManager<User> _userManager;

        public SeedDb(DataContext context,
                      IUserHelper userHelper)
                       //UserManager<User> userManager)
        {
            _context = context;
            _random = new Random();
            _userHelper = userHelper;
            //_userManager = userManager;
        }

    public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); //If there is no DB created, this will create one
                                                          //await _context.Database.MigrateAsync(); //If there is no DB, it will create one and execute the migrations 

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Worker");
            await _userHelper.CheckRoleAsync("Customer");
            await _userHelper.CheckRoleAsync("Anonymous");

            var user = await _userHelper.GetUserByEmailAsync("cristinajular@gmail.com");
            //var user = await _userManager.FindByIdAsync("cristinajular@gmail.com"); //check if this user has been already created
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Cristina",
                    LastName = "Jular",
                    Email = "cristinajular@gmail.com",
                    UserName = "cristinajular@gmail.com",
                    PhoneNumber = "914412891"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                //var result = await _userManager.CreateAsync(user, "123456"); //Create User through the _userManager's method and the pass goes separetely in case I want to encrypt it
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("It was not possible to create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                //await _userHelper.AddUserToRoleAsync(user, "Worker");
                //await _userHelper.AddUserToRoleAsync(user, "Customer");
                //await _userHelper.AddUserToRoleAsync(user, "Anonymous");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
                //await _userHelper.AddUserToRoleAsync(user, "Worker");
                //await _userHelper.AddUserToRoleAsync(user, "Customer");
                //await _userHelper.AddUserToRoleAsync(user, "Anonymous");
            }


            if (!_context.Customers.Any())
            {
                AddCustomer("Juan", "Martin", user);
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

        private void AddCustomer(string first, string last, User user)
        {
            _context.Customers.Add(new Customer
            {
                FirstName = first,
                LastName = last,
                NIF_customer = _random.Next(100000000),
                Telefone = _random.Next(900000000),
                Email = "kamistesta@gmail.com",
                User = user
            });
        }
    }
}
