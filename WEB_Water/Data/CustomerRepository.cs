using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;

namespace WEB_Water.Data
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public CustomerRepository(DataContext context,
                                  IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public IQueryable GetAllWithUsers() //bring all the customers with the user (INNER JOIN)
        {
            return _context.Customers.Include(c => c.User);
        }


        //public async Task GetAddressesForCustomerAsync(string mail)
        //{
        //    var user = await _userHelper.GetUserByEmailAsync(mail);
        //    var addresses = await _context.Addresses.Where(mor => mor.Customer == customer);
        //}
    }
}
