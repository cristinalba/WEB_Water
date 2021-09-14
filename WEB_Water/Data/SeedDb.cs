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
            //await _userHelper.CheckRoleAsync("Worker");
            await _userHelper.CheckRoleAsync("Customer");
            //await _userHelper.CheckRoleAsync("Anonymous");

            var user = await _userHelper.GetUserByEmailAsync("admin@webwater");
            //var user = await _userManager.FindByIdAsync("admin@webwater"); //check if this user has been already created

            var user1 = await _userHelper.GetUserByEmailAsync("admin@webwater");
            var user2 = await _userHelper.GetUserByEmailAsync("admin@webwater");
            var user3 = await _userHelper.GetUserByEmailAsync("admin@webwater");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Administrator",
                    LastName = "Web_Water",
                    Email = "admin@webwater",
                    UserName = "admin@webwater",
                    PhoneNumber = "914412891",
                    Nif = _random.Next(100000000, 999999999).ToString()
                };
                user1 = new User
                {
                    FirstName = "Paquito",
                    LastName = "Chocolatero",
                    Email = "paco@webwater.com",
                    UserName = "paco@webwater.com",
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
                    Nif = _random.Next(100000000, 999999999).ToString()
                };


                user3 = new User
                {
                    FirstName = "King",
                    LastName = "Africa",
                    Email = "king@webwater.com",
                    UserName = "king@webwater.com",
                    PhoneNumber = "9" + _random.Next(10000000, 99999999).ToString(),
                    Nif = _random.Next(100000000, 999999999).ToString()
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
                await _userHelper.AddUserToRoleAsync(user1, "Admin");
                await _userHelper.AddUserToRoleAsync(user2, "Admin");
                await _userHelper.AddUserToRoleAsync(user3, "Admin");
            }


            if (!_context.Readers.Any()) // se estiver vazia
            {
                this.AddEquipment("X Street", user1);
                this.AddEquipment("Y Street", user2);
                this.AddEquipment("Z Street", user3);

                await _context.SaveChangesAsync(); //vamos gravar

            }

        }

        private void AddEquipment(string address, User user)
        {
            _context.Readers.Add(new Reader
            {
                ReaderName = "Ref" + _random.Next(100000000, 999999999).ToString(),
                AddressName = address,
                User = user
            });
        }
    }
}
