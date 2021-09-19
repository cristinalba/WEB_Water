using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;

namespace WEB_Water.Data.Repositories
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public BillRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Bill>> GetBillAsync(string username)
        {

            var user = await _userHelper.GetUserByIdAsync(username);
            if (user == null)
            {
                return null;
            }

            return _context.Bills
               .Include(r => r.Reader)
               .Include(x => x.Reading)
               .Where(u => u.User == user)
               .OrderByDescending(b => b.BillDate);

        }

        public bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.Reading.Id == id);
        }

      
    }
}
