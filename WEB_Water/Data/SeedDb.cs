using Microsoft.AspNetCore.Identity;
using System;
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
           
            var user = await _userHelper.GetUserByEmailAsync("admin@webwater.com");//check if this user has been already created
            //var user = await _userManager.FindByIdAsync("admin@webwater"); 

            var user1 = await _userHelper.GetUserByEmailAsync("adminworker1@webwater.com");
            var user2 = await _userHelper.GetUserByEmailAsync("quim@webwater.com");
            var user3 = await _userHelper.GetUserByEmailAsync("king@webwater.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Administrator",
                    LastName = "Web_Water",
                    Email = "admin@webwater.com",
                    UserName = "admin@webwater.com",
                    PhoneNumber = "914412891",
                    Nif = _random.Next(100000000, 999999999).ToString()
                };
                user1 = new User
                {
                    FirstName = "Admin_Worker",
                    LastName = "Web_Water",
                    Email = "adminworker1@webwater.com",
                    UserName = "adminworker1@webwater.com",
                    PhoneNumber = "9" + _random.Next(10000000, 99999999).ToString(),
                    Nif = _random.Next(100000000, 999999999).ToString()
                };


                user2 = new User
                {                 
                    FirstName = "Quim",
                    LastName = "Barreiros",
                    Email = "quim@webwater.com",
                    UserName = "quim@webwater.com",
                    PhoneNumber = "9" + _random.Next(10000000, 99999999).ToString(),
                    Nif = _random.Next(100000000, 999999999).ToString(),
                    IsCustomer = true
                };


                user3 = new User
                {
                    FirstName = "King",
                    LastName = "Africa",
                    Email = "king@webwater.com",
                    UserName = "king@webwater.com",
                    PhoneNumber = "9" + _random.Next(10000000, 99999999).ToString(),
                    Nif = _random.Next(100000000, 999999999).ToString(),
                    IsCustomer = true 
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                //var result = await _userManager.CreateAsync(user, "123456"); //Create User through the _userManager's method and the pass goes separetely in case I want to encrypt it
                var result1 = await _userHelper.AddUserAsync(user1, "123456");
                var result2 = await _userHelper.AddUserAsync(user2, "123456");
                var result3 = await _userHelper.AddUserAsync(user3, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("It was not possible to create the user in seeder");
                }
                if (result1 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("It was not possible to create the user in seeder");
                }
                if (result2 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("It was not possible to create the user in seeder");
                }
                if (result3 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("It was not possible to create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                await _userHelper.AddUserToRoleAsync(user1, "Worker");
                await _userHelper.AddUserToRoleAsync(user2, "Customer");
                await _userHelper.AddUserToRoleAsync(user3, "Customer");
               
                //await _userHelper.AddUserToRoleAsync(user, "Anonymous");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
               
            }
            var isInRole1 = await _userHelper.IsUserInRoleAsync(user1, "Worker");
            if (!isInRole1)
            {
                await _userHelper.AddUserToRoleAsync(user1, "Worker");

            }
            var isInRole2 = await _userHelper.IsUserInRoleAsync(user2, "Customer");
            if (!isInRole2)
            {
                await _userHelper.AddUserToRoleAsync(user2, "Customer");

            }

            var isInRole3 = await _userHelper.IsUserInRoleAsync(user3, "Customer");
            if (!isInRole3)
            {
                await _userHelper.AddUserToRoleAsync(user3, "Customer");

            }

            if (!_context.Readers.Any()) // if it is empty
            {
                //this.AddReader("X Street", user1);
                this.AddReader("Y Street", user2);
                this.AddReader("Z Street", user3);

                await _context.SaveChangesAsync(); //vamos gravar

            }

        }

        private void AddReader(string address, User user)
        {
            _context.Readers.Add(new Reader
            {
                ReaderName = "Ref" + _random.Next(100000000, 999999999).ToString(),
                AddressName = address,
                Installation = DateTime.Now,
                User = user
            });
        }
    }
}
